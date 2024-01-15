using System;
using ITPRO.Rover.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.Controller
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private float charge = 1200; //1200 - заряд примерно на 20 минут езды
        [SerializeField] private float maxCharge = 1200;
        [SerializeField] private float speedDischarge = 1;
        [SerializeField] private RoverController rover;
        [SerializeField] private Image batteryBar;
        [SerializeField] private TextMeshProUGUI batteryText;
        
        private float _consumption = 1;

        private void OnEnable()
        {
            rover.StateEvent += CheckConsumption;
        }

        private void OnDisable()
        {
            rover.StateEvent -= CheckConsumption;
        }

        /// <summary>
        /// Расчет потребления
        /// </summary>
        /// <param name="state"></param>
        /// <param name="speed"></param>
        private void CheckConsumption(States state, float speed)
        {
            switch (state)
            {
                case States.stay:
                    _consumption = 0.5f;
                    break;
                case States.acceleration:
                    _consumption = 1 + Math.Abs(rover.GetTargetSpeed() - speed)/rover.GetMaxSpeed();
                    break;
                case States.move:
                    if (Mathf.Abs(rover.GetMaxSpeed() - speed) < 0.5f)
                    {
                        _consumption = Math.Abs(speed) * 1.5f / rover.GetMaxSpeed();
                    }
                    else
                    {
                        _consumption = Math.Abs(speed) / rover.GetMaxSpeed();
                    }
                    break;
                case States.breaking:
                    _consumption = 2f;
                    break;
            }
        }

        private void Update()
        {
            charge -= _consumption * Time.deltaTime * speedDischarge;
            batteryBar.fillAmount = charge / maxCharge;
            batteryText.SetText(Math.Round(charge/maxCharge*100) + "%");
            if (charge <= 0)
            {
                GameManager.instance.Lose("кончилась энергия");
            }
        }

        public float GetCharge()
        {
            return charge / maxCharge;
        }
    }
}