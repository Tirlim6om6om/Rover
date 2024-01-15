using System;
using System.Collections;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ITPRO.Rover.Managers
{
    public class OutScene : MonoBehaviour
    {
        [SerializeField] private GameObject outMenu;
        [SerializeField] private Image circleOut;
        private IEnumerator _outIEnumerator;
                
        private void Start()
        {
            JoystickInput.joy.Controller.Out.performed += context => StartOut(context);
            JoystickInput.joy.Controller.Out.canceled += context => StartOut(context);
        }

        private void StartOut(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                outMenu.SetActive(true);
                _outIEnumerator = Out();
                StartCoroutine(_outIEnumerator);
            }
            else
            {
                outMenu.SetActive(false);
                StopCoroutine(_outIEnumerator);
            }
        }

        private IEnumerator Out()
        {
            float progress = 0;
            while (JoystickInput.joy.Controller.Out.IsPressed())
            {
                progress += Time.fixedDeltaTime / 1.5f;
                circleOut.fillAmount = progress;
                if (progress >= 1)
                {
                    StatisticWriter.instance.FailReason = "Выход из приложения";
                    StatisticWriter.instance.SaveJson();
                    yield return new WaitForSeconds(0.1f);
                    SceneController.instance.LoadLevel(0);
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}