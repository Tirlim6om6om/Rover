using System;
using UnityEngine;

namespace ITPRO.Rover.RoboticArm
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] private RoboticArm arm;

        private Light _light;
        
        private void Start()
        {
            TryGetComponent(out _light);
            Activate(false);
            arm.CollectActive += Activate;
        }

        private void Activate(bool active)
        {
            _light.enabled = active;
        }
    }
}