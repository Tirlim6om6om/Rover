using System;
using ITPRO.Rover.Test;
using ITPRO.Rover.UI;
using UnityEngine;

namespace ITPRO.Rover.Managers
{
    public class TimeController : MonoBehaviour
    {
        public bool godMode;
        public static TimeController instance;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
#if UNITY_EDITOR
            godMode = true;
#endif
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F11) && Input.GetKey(KeyCode.PageUp))
            {
                if (godMode == false)
                {
                    godMode = true;
                    Notification.instance.SetNotification("Установлен админ режим:)");
                }
            }
            if(!godMode) return;
            if (Input.GetKeyDown(KeyCode.K))
            {
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Time.timeScale = 8;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                TestController.instance.GetActualTest().test.Deactivate();
            }
        }

        public void SetDefault()
        {
            if (godMode)
            {
                Time.timeScale = 1;
            }
        }
    }
}