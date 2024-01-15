using System;
using System.Collections;
using UnityEngine;


namespace ITPRO.Rover.Controller
{
    public class JunctionConnection : MonoBehaviour
    {
        [SerializeField] private Wheels wheels;
        private bool _active;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Junction_zone"))
            {
                if(!_active)
                    wheels.SetConnectionJunction(true);
                _active = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Junction_zone"))
            {
                if (_active)
                {
                    StartCoroutine(CheckActive());
                }
                _active = false;
            }
        }

        private IEnumerator CheckActive()
        {
            yield return new WaitForSeconds(1);
            if (!_active)
            {
                wheels.SetConnectionJunction(false);
            }
        }
    }
}