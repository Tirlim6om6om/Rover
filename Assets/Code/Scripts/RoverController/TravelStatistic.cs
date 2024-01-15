using System;
using System.Collections;
using System.Collections.Generic;
using ITPRO.Rover.Managers;
using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.Controller
{
    public class TravelStatistic : MonoBehaviour
    {
        private List<bool> active = new List<bool>(){true,false,false, false};
        private Battery _battery;
        private int _step = 0;
        private float _oldCharge = 1;
        
        private void Start()
        {
            TryGetComponent(out _battery);
            StartCoroutine(CheckingDistance(0));
        }
        
        public void NextPoint()
        {
            active[_step] = false;
            _step += 1;
            if (_step < 3)
            {
                active[_step] = true;
            }
            StartCoroutine(CheckingDistance(_step));
        }

        public IEnumerator CheckingDistance(int index)
        {
            float distance = 0;
            Vector3 oldpos = transform.position;
            while (active[index])
            {
                distance += Vector3.Distance(transform.position, oldpos);
                oldpos = transform.position;
                yield return new WaitForSeconds(0.05f);
            }

            switch (index)
            {
                case 0:
                    StatisticWriter.instance.FirstTraveledDistance = (float)Math.Round(distance);
                    StatisticWriter.instance.ActiveFirstPoint = Timer.instance.GetTime();
                    StatisticWriter.instance.ChargeFirstPoint = Math.Round((_oldCharge - _battery.GetCharge())*100) + "%";
                    _oldCharge = _battery.GetCharge();
                    break;
                case 1:
                    StatisticWriter.instance.SecondTraveledDistance = (float)Math.Round(distance);
                    StatisticWriter.instance.ActiveSecondPoint = Timer.instance.GetTime();
                    StatisticWriter.instance.ChargeSecondPoint = Math.Round((_oldCharge - _battery.GetCharge())*100) + "%";
                    _oldCharge = _battery.GetCharge();
                    break;
                case 2:
                    StatisticWriter.instance.ThirdTraveledDistance = (float)Math.Round(distance);
                    StatisticWriter.instance.ActiveThirdPoint = Timer.instance.GetTime();
                    StatisticWriter.instance.ChargeThirdPoint = Math.Round((_oldCharge - _battery.GetCharge())*100) + "%";
                    _oldCharge = _battery.GetCharge();
                    break;
            }
        }

        public void JunctionConnect()
        {
            if (_step >= 3)
            {
                StatisticWriter.instance.CompletionDocking =  Timer.instance.GetTime();
                GameManager.instance.Win();
            }
        }
    }
}
