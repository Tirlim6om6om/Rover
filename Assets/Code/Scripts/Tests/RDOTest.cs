 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ITPRO.Rover.User;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITPRO.Rover.Test.RDO
{
    public class RDOTest : TestBase
    {
        public delegate void TestResultProvider();
        public event TestResultProvider OnTestEnd;
        
        [SerializeField] private GameObject testInterface; 
        [SerializeField] private TextMeshProUGUI titleText; 

        [SerializeField] private Transform pivotPoint;
        [SerializeField] private Transform target;

        [SerializeField] private TextMeshProUGUI resultField;

        [SerializeField] public List<string> results = new List<string>();
        [SerializeField, Range(0, 2)] private int rdoNumber;
        public List<int> lateOfIterations = new List<int>(){ 2, 2, 3, 3, 2, 3, 3, 2, 3, 2 };

        public List<int> reliability = new List<int>();
        public List<string> deviations = new List<string>();

        private bool _isRotating;

        private float _seconds;

        private Vector3 _z1;
        private Vector3 _z2;

        private decimal _timer;

        private int _iteration;
        private const int NumOfIteration = 3;

        private string _circleTouchedResult;

        private Timer.GameTime _activateTime;
        private RdoAttempt _attempt;

        private bool _turnOff;

        private void Start()
        {
            JoystickInput.joy.Controller.Trigger.performed += OnTrigger;
            JoystickInput.joy.Controller.Trigger.started += OnTrigger;
            JoystickInput.joy.Controller.Trigger.canceled += OnTrigger;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            
            _iteration = 0;
            _timer = 0;
            _isRotating = false;
            _turnOff = false;
            results = new List<string>();
            reliability = new List<int>();
            StartCoroutine(WaitForNextIteration(0));
        }

        private void OnTrigger(InputAction.CallbackContext context)
        {
            if (!_isRotating || !Active || _turnOff) return;
            if (context.ReadValue<float>() < 0.5f) return;
            _isRotating = false;
            string result = GetIterationResult();

            _iteration++;
            results.Add(result);

            if (_iteration >= NumOfIteration)
            {
                resultField.text = _circleTouchedResult;
                _turnOff = true;
                Invoke("TurnOff", 1f);
                return;
            }

            StartCoroutine(WaitForNextIteration(1));
        }

        private void TurnOff()
        {
            OnTestEnd?.Invoke();
            resultField.text = _circleTouchedResult;
            Deactivate();
        }

        private void Update()
        {
            if (!_isRotating || !Active) return;
            pivotPoint.Rotate(-Vector3.forward * (Time.deltaTime * (360f / _seconds)));
            _timer += (decimal) Time.deltaTime;
        }

        public override bool Activate()
        {
            if (Active) return false;
            
            enabled = true;
            transform.parent.gameObject.SetActive(true);
            Active = true;
            testInterface.SetActive(true);
            
            deviations = new List<string>();
            _activateTime = Timer.instance.time;
            _attempt.startTime = _activateTime.ToString("mm:ss");
            activateTest.Invoke(Timer.instance.time.ToString("mm:ss"));
            return true;
        }

        public override bool Deactivate()
        {
            if (!Active) return false;
            
            enabled = false;
            transform.parent.gameObject.SetActive(false);
            Active = false;
            testInterface.SetActive(false);
            
            _attempt.endTime = Timer.instance.time.ToString("mm:ss");
            _attempt.executionTime = Timer.SubtractionTime(Timer.instance.time, _activateTime).ToString("mm:ss");
            _attempt.successfulAttempts = reliability.Sum();
            _attempt.deviations = deviations;
            
            StatisticWriter.instance.AddRdoAttempt(rdoNumber, _attempt);
            rdoNumber++;
            
            deactivateTest.Invoke(Timer.instance.time.ToString("mm:ss"));
            return true;
        }

        private string GetIterationResult()
        {
            string result;

            _z1 = target.transform.eulerAngles;
            _z2 = pivotPoint.transform.eulerAngles;

            if (Math.Abs(_z1.z - _z2.z) < 10.8)
            {
                reliability.Add(1);
                _circleTouchedResult = "Вы попали в диапазон";
            }
            else
            {
                reliability.Add(0);
                _circleTouchedResult = "Вы не попали в диапазон";
            }

            decimal tempRes = Math.Round(_timer - (decimal)_seconds, 3);
            string sign = tempRes > 0 ? "+" : "";
            print("add: " + sign + " : " + tempRes);
            deviations.Add($"{sign}{tempRes}");

            if (((_z1.z > 0) && _z2.z >= _z1.z - 3 && _z2.z <= _z1.z + 3) ||
                ((_z1.z < 0) && -(_z2.z) >= -((_z1.z) - 3) && -(_z2.z) <= -((_z1.z) + 3))) 
                result = $"Успех  {sign}{tempRes} IterTime: {(lateOfIterations[_iteration])}";
            else result = $"Провал {sign}{tempRes} IterTime: {(lateOfIterations[_iteration])}";
            return result;
        }

        private void Rotate(float seconds)
        {
            transform.eulerAngles = new Vector3(0, 0, target.transform.eulerAngles.z);
            _seconds = seconds;
            _isRotating = true;
        }

        private void SpawnPoints()
        {
            float startRotation = UnityEngine.Random.Range(-360, 0);
            pivotPoint.eulerAngles = new Vector3(0, 0, startRotation);
            target.eulerAngles = new Vector3(0, 0, startRotation);
        }

        private IEnumerator WaitForNextIteration(int waitingTime)
        {
            _timer = 0;

            if (_iteration != 0) resultField.text = _circleTouchedResult;

            yield return new WaitForSeconds(waitingTime);
            resultField.text = string.Empty;

            titleText.text = $"Попытка {_iteration + 1}/{NumOfIteration}";
            SpawnPoints();

            yield return new WaitUntil(() => JoystickInput.joy.Controller.Trigger.IsPressed());
            Rotate(lateOfIterations[_iteration]);
        }

        private void OnDestroy()
        {
            OnTestEnd = null;
        }
    }
}
