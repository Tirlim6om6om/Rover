using System.Collections;
using System.Collections.Generic;
using ITPRO.Rover.User;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ITPRO.Rover.Test.Raven
{
    public class RavenMatricesTest : TestBase
    {
        [SerializeField] private GameObject testInterface;
        [SerializeField] private RavenMatrices ravenMatrices;
        [SerializeField] private RavenCursor cursor;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private Image[] matrixImages;
        [SerializeField] private AnswerPosition[] answerPositions;
        [SerializeField] private InputActionProperty interactionKey;

        private List<Matrix> _matrices;
        private int _matricesNumber;
        private RectTransform _questionTransform;
        private RavenAttempt _attempt;
        private Timer.GameTime _activateTime;
        private int _errors;
        private int _count;

        private void Start()
        {
            _questionTransform = matrixImages[0].GetComponent<RectTransform>();
        }

        public override bool Activate()
        {
            if (Active) return false;

            cursor.MoveCursor();
            UpdateMatrices();
            StartCoroutine(WaitAnswer());
            testInterface.SetActive(true);
            Active = true;
            _activateTime = Timer.instance.time;
            _attempt.startTime = _activateTime.ToString("mm:ss");
            _errors = 0;
            _count = 0;
            activateTest.Invoke(Timer.instance.time.ToString("mm:ss"));
            return true;
        }

        public override bool Deactivate()
        {
            if (!Active) return false;
            _matricesNumber = 0;
            _matrices = null;
            testInterface.SetActive(false);
            Active = false;
            _attempt.reliabilityFactor = (_count - _errors) / (float)_count;
            Timer.GameTime speed = Timer.SubtractionTime(Timer.instance.time, _activateTime);
            _attempt.speedFactor = _count / (float)speed.ToSeconds();
            _attempt.endTime = Timer.instance.time.ToString("mm:ss");
            StatisticWriter.instance.AddRavenAttempt(_attempt);

            deactivateTest.Invoke(Timer.instance.time.ToString("mm:ss"));
            return true;
        }

        private IEnumerator WaitAnswer()
        {
            for (_matricesNumber = 0; _matricesNumber < 5;)
            {
                yield return new WaitUntil(() =>
                    JoystickInput.GetButton(interactionKey.action) || Input.GetMouseButtonDown(0));
                yield return null;

                if (!cursor.SelectImage) continue;

                if (_matrices![_matricesNumber].rightAnswer == cursor.SelectImage.Number)
                {
                    _matricesNumber++;
                    foreach (AnswerPosition item in answerPositions)
                    {
                        if (!item.RightPosition(_matricesNumber)) continue;

                        item.VisualizeAnswer(cursor.SelectImage.AnswerImage.sprite);
                        yield return new WaitForSeconds(1);
                        item.HideAnswer();
                        break;
                    }
                }
                else
                {
                    _errors++;
                    _matrices = null;
                }

                _count++;
                UpdateMatrices();
                yield return new WaitUntil(() => JoystickInput.joy.Controller.Trigger.ReadValue<float>() == 0);
            }

            Deactivate();
        }

        private void UpdateMatrices()
        {
            if (_matricesNumber >= 5) return;

            _matrices ??= ravenMatrices.GetRandomMatrices();

            //Не работало при билде!
            //var objects =
            //    AssetDatabase.LoadAllAssetRepresentationsAtPath(
            //        AssetDatabase.GetAssetPath(_matrices[_matricesNumber].texture));

            Sprite[] objects = Resources.LoadAll<Sprite>("RavenMatrices/" + _matrices[_matricesNumber].texture.name);

            for (int i = 0; i < matrixImages.Length; i++)
            {
                if (i < objects.Length)
                {
                    matrixImages[i].gameObject.SetActive(true);
                    matrixImages[i].sprite = objects[i];
                    continue;
                }

                matrixImages[i].gameObject.SetActive(false);
            }

            Vector2 size = new Vector2(1100,
                1100 / matrixImages[0].sprite.bounds.size.x * matrixImages[0].sprite.bounds.size.y);

            gridLayout.cellSize = matrixImages[1].sprite.bounds.size * 1100 / matrixImages[0].sprite.bounds.size.x;
            _questionTransform.sizeDelta = size;
        }
    }
}