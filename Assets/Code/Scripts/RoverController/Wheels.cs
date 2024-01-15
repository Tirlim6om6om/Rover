using System;
using System.Collections;
using UnityEngine;

namespace ITPRO.Rover.Controller
{
    public class Wheels : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] wheels;
        private Vector3[] _centers = new Vector3[6];
        private float[] _distances = new float[6];
        private bool _activeJunction = false;


        private void Awake()
        {
            int i = 0;
            foreach (var wheel in wheels)
            {
                _centers[i] = wheel.center;
                _distances[i] = wheel.suspensionDistance;
                i++;
            }
        }

        public bool CheckGround()
        {
            foreach (var wheel in wheels)
            {
                if (wheel.isGrounded)
                {
                    return true;
                }
            }
            return false;
        }
        
        public void SetConnectionJunction(bool active)
        {
            _activeJunction = active;
            if (active)
            {
                StartCoroutine(HardWheels());
            }
            else
            {
                int i = 0;
                foreach (var wheel in wheels)
                {
                    wheel.center = _centers[i];
                    wheel.suspensionDistance = _distances[i];
                    i++;
                }
            }
        }


        private IEnumerator HardWheels()
        {
            while (_activeJunction)
            {
                int i = 0;
                foreach (var wheel in wheels)
                {
                    if (wheel.center.y > -0.125f)
                    {
                        wheel.center -= new Vector3(0, 0.005f, 0);
                    }
                    else
                    {
                        wheel.center = new Vector3(0, -0.125f, 0);
                    }
                    if (wheel.suspensionDistance > 0.025)
                    {
                        wheel.suspensionDistance -= 0.005f;
                    }
                    else
                    {
                        wheel.suspensionDistance = 0.025f;
                    }

                    i++;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}