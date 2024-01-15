using System;
using System.Collections;
using ITPRO.Rover.Controller;
using ITPRO.Rover.Test;
using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.Crash
{
    public class DistanceCheck : MonoBehaviour
    {
        [SerializeField] private RoverController rover;
        [SerializeField] private Speedometer speedometer;
        [SerializeField] private float thresholdSpeed;
        [SerializeField] private float thresholdDistance;
        private bool _active;

        private void OnEnable()
        {
            rover.StateEvent += CheckSpeed;
        }

        private void OnDisable()
        {
            rover.StateEvent -= CheckSpeed;
        }

        public void CheckSpeed(States state, float speed)
        {
            if (speed > thresholdSpeed)
            {
                if (!_active)
                {
                    _active = true;
                    StartCoroutine(DistanceCalculation());
                }
            }
            else
            {
                _active = false;
            }
        }
        
        IEnumerator DistanceCalculation()
        {
            float distance = 0;
            while (_active)
            {
                distance += speedometer.GetSpeedMeter();
                if (distance > thresholdDistance)
                {
                    StatisticWriter.instance.ControlErrors++;
                    TestController.instance.ActivateTest(TypeTest.wheelFialure);
                    _active = false;
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}