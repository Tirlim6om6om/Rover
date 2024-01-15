using System;
using ITPRO.Rover.Controller;
using UnityEngine;


namespace ITPRO.Rover.Minerals
{
    public class WheelCrash : MonoBehaviour
    {
        [SerializeField] private RoverController rover;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Mineral mineral))
            {
                if (Mathf.Abs(rover.GetRealSpeed()) > 0.5f)
                {
                    mineral.Crash();
                }
            }
        }
    }
}