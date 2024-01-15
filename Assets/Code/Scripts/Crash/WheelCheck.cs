using System;
using ITPRO.Rover.Controller;
using ITPRO.Rover.Test;
using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.Crash
{
    [RequireComponent(typeof(WheelCollider))]
    public class WheelCheck : MonoBehaviour
    {
        [SerializeField] private Speedometer speed;
        [SerializeField] private LayerMask layer;
        private WheelCollider _collider;
        
        private void Start()
        {
            TryGetComponent(out _collider);
        }
        
        private void Update()
        {
            if (_collider.GetGroundHit(out WheelHit hit))
            {
                if (((1<<hit.collider.gameObject.layer) & layer) != 0)
                {
                    if (speed.GetSpeed() >= 10)
                    {
                        StatisticWriter.instance.ControlErrors++;
                        TestController.instance.ActivateTest(TypeTest.wheelFialure);
                    }
                }
            }
        }
    }
}
