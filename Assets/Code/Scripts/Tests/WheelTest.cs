using System.Collections.Generic;
using System.Collections;
using ITPRO.Rover.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace ITPRO.Rover.Test.Wheel
{
    public class WheelTest : TestBase
    {
        [System.Serializable]
        private struct WheelIcon
        {
            [SerializeField] private Image icon;
            [SerializeField] private InputActionProperty repairKey;

            public InputActionProperty RepairKey
            {
                get => repairKey;
                private set => repairKey = value;
            }
            
            public void ChangeColor(Color color) => icon.color = color;
        }

        [SerializeField] private List<WheelIcon> wheelIcons;
        [SerializeField] private Color brokenColor;
        [SerializeField] private GameObject testInterface;
        [SerializeField] private TMP_Text informationText;

        private WheelAttempt _attempt;
        private int _brokenWheelNumber;
        private readonly Color _defaultColor = Color.white;
        private int _errors;
        private Timer.GameTime _activateTime;

        private void OnEnable()
        {
            foreach (WheelIcon icon in wheelIcons)
            {
                icon.RepairKey.action.Enable();
            }
        }

        public override bool Activate()
        {
            if (Active) return false;
            
            _brokenWheelNumber = Random.Range(0, 5);
            informationText.text = $"Поломка колеса №{_brokenWheelNumber + 1}";
            StartCoroutine(WaitAnyKey());
            Active = true;
            testInterface.SetActive(true);
            _activateTime = Timer.instance.time;
            _attempt.detectionTime = _activateTime.ToString("mm:ss");
            activateTest.Invoke(_activateTime.ToString("mm:ss"));
            return true;
        }

        public override bool Deactivate()
        {
            if (!Active) return false;
            informationText.text = string.Empty;
            wheelIcons[_brokenWheelNumber].ChangeColor(_defaultColor);
            testInterface.SetActive(false);
            Active = false;
            _attempt.correctionTime = Timer.instance.time.ToString("mm:ss");
            _attempt.executionTime = Timer.SubtractionTime(Timer.instance.time, _activateTime).ToString("mm:ss");
            switch (_errors)
            {
                case 0:
                    _attempt.firstError = false;
                    _attempt.secondError = false;
                    break;
                case 1:
                    _attempt.firstError = true;
                    _attempt.secondError = false;
                    break;
                case >1:
                    _attempt.firstError = true;
                    _attempt.secondError = true;
                    break;
            }
            StatisticWriter.instance.AddWheelAttempt(_attempt);
            deactivateTest.Invoke(Timer.instance.time.ToString("mm:ss"));
            return true;
        }

        private IEnumerator WaitAnyKey()
        {
            yield return new WaitUntil(KeyBePressed);
            
            if (wheelIcons[_brokenWheelNumber].RepairKey.action.IsPressed())
            {
                _errors = 0;
                Deactivate();
                yield break;
            }

            testInterface.SetActive(true);
            wheelIcons[_brokenWheelNumber].ChangeColor(brokenColor);

            var buttons = new List<int>() { 0, 1, 2, 3, 4, 5 };
            
            while (!wheelIcons[_brokenWheelNumber].RepairKey.action.IsPressed())
            {
                foreach (int button in buttons)
                {
                    if (wheelIcons[button].RepairKey.action.IsPressed())
                    {
                        print("test");
                        buttons.Remove(button);
                        break;
                    }
                }
                yield return null;
            }
            _errors = wheelIcons.Count - buttons.Count;

            if (_errors > wheelIcons.Count) _errors = wheelIcons.Count;
            Deactivate();
        }

        private bool KeyBePressed()
        {
            foreach (WheelIcon icon in wheelIcons)
            {
                if (!icon.RepairKey.action.IsPressed()) continue;
                return true;
            }
            return false;
        }
    }
}
