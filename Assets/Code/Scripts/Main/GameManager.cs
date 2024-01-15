using System;
using System.Collections;
using ITPRO.Rover.Controller;
using ITPRO.Rover.User;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ITPRO.Rover.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        [SerializeField] private int minutes;
        [SerializeField] private bool limitTime;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameObject LoseUI;
        [SerializeField] private TextMeshProUGUI textLose;
        [SerializeField] private GameObject WinUI;
        [SerializeField] private RoverController rover;
        [SerializeField] private GameObject otherDisplaysUI;
        
        private Timer.GameTime _time;
        private bool _active = true;

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            if (limitTime)
            {
                _time.Minutes = minutes;
                StartCoroutine(TimerGame());
                timeText.gameObject.SetActive(true);
                timeText.SetText(_time.ToString("mm:ss"));
            }
            else
            {
                timeText.gameObject.SetActive(false);
            }

            StartCoroutine(Autosave());
        }

        private IEnumerator TimerGame()
        {
            while ((_time.Minutes != 0 || _time.Seconds != 0) && _active)
            {
                yield return new WaitForSeconds(1);
                _time.Seconds--;
                timeText.SetText(_time.ToString("mm:ss"));
            }
            Lose("время истекло");
        }

        private IEnumerator Autosave()
        {
            while (_active)
            {
                StatisticWriter.instance.SaveJson();
                yield return new WaitForSeconds(5);
            }
        }

        public void Lose(string reason)
        {
            if(!_active) return;
            _active = false;
            Debug.Log("Lose");
            StatisticWriter.instance.ExecutionDuration = Timer.instance.time.ToString("mm:ss");
            StatisticWriter.instance.FailReason = reason;
            StatisticWriter.instance.Succeeded = false;
            StatisticWriter.instance.SaveJson();
            textLose.SetText(reason);
            LoseUI.SetActive(true);
            SetCursor(true);
            rover.SetLock(true);
            otherDisplaysUI.SetActive(false);
        }

        public void Win()
        {
            if(!_active) return;
            _active = false;
            Debug.Log("Win");
            StatisticWriter.instance.ExecutionDuration = Timer.instance.time.ToString("mm:ss");
            StatisticWriter.instance.Succeeded = true;
            StatisticWriter.instance.SaveJson();
            WinUI.SetActive(true);
            SetCursor(true);
            rover.SetLock(true);
            otherDisplaysUI.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            StatisticWriter.instance.ExecutionDuration = Timer.instance.time.ToString("mm:ss");
            StatisticWriter.instance.FailReason = "Выход из приложения";
            StatisticWriter.instance.SaveJson();
        }

        private void SetCursor(bool active)
        {
            VirtualCursor.instance.SetVisible(active);
            return;
            UnityEngine.Cursor.visible = active;
            if(active)
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            else
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Mouse.current.WarpCursorPosition(Screen.safeArea.center);
            InputState.Change(Mouse.current.position, Screen.safeArea.center);
        }

        public bool GetActive()
        {
            return _active;
        }
    }
}
