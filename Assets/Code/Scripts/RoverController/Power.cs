using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ITPRO.Rover.Controller
{
    public class Power : MonoBehaviour
    {
        [SerializeField] private RoverController rover;
        [SerializeField] private Image powerBar;
        [SerializeField] private TextMeshProUGUI powerText;
        
        private void OnEnable()
        {
            rover.StateEvent += CheckPower;
        }

        private void OnDisable()
        {
            rover.StateEvent -= CheckPower;
        }

        public void CheckPower(States state, float speed)
        {
            float power = 0;
            switch (state)
            {
                case States.stay:
                    power = Math.Abs(speed) / rover.GetMaxSpeed();
                    break;
                case States.acceleration:
                    power = (Math.Abs(speed)+0.1f) / rover.GetMaxSpeed();
                    break;
                case States.move:
                    power = (Math.Abs(speed)+0.1f) / rover.GetMaxSpeed();
                    break;
                case States.breaking:
                    power = Math.Abs(speed) / rover.GetMaxSpeed();
                    break;
            }
            if (power >= 1) power = 1;
            powerBar.fillAmount = power;
            powerText.SetText(Math.Round(power*100) + "%");
        }
    }
}
