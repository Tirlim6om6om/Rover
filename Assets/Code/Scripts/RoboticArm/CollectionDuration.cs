using System;
using System.Collections;
using ITPRO.Rover.Minerals;
using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.RoboticArm
{
    public class CollectionDuration : MonoBehaviour
    {
        [SerializeField] private MineralSpawn spawn;
        [SerializeField] private Transform rover;
        private bool _active;
        private  Timer.GameTime _time = new Timer.GameTime();
        
        private void Start()
        {
            if (TryGetComponent(out RoboticArm arm))
            {
                arm.CollectActive += CheckTime;
            }
        }

        private void CheckTime(bool collectMode)
        {
            if(collectMode && GetCloserMinerals() > 5) return;
            _active = collectMode;
            if (collectMode)
            {
                StartCoroutine(TimerCollect());
            }
        }

        private float GetCloserMinerals()
        {
            float min = 100000;
            foreach (var spawned in spawn.spawned)
            {
                float dist = Vector3.Distance(rover.position, spawned.transform.position);
                if (dist < min)
                {
                    min = dist;
                }
            }

            return min;
        }

        private IEnumerator TimerCollect()
        {
            while (_active)
            {
                yield return new WaitForSeconds(1);
                _time.Seconds++;
            }
            StatisticWriter.instance.CollectionDuration = _time.ToString("mm:ss");
        }
    }
}