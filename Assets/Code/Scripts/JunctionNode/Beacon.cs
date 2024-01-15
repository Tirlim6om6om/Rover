using System;
using System.Collections;
using UnityEngine;

namespace ITPRO.Rover.Junction
{
    public class Beacon : MonoBehaviour
    {

        [SerializeField] private MeshRenderer beacon;

        private Material _material;
        private bool _active;
        private Color _color;
        
        private void Start()
        {
            _material = beacon.material;
            _color  = _material.GetColor("_EmissionColor");
        }

        public void SetActive(bool active)
        {
            _active = active;
            if (active)
            {
                StartCoroutine(Blink());
            }
            else
            {
                SetColorIntensity(0);
                
            }
        }


        private IEnumerator Blink()
        {
            bool rise = true;
            float value = 0;
            while (_active)
            {
                if (rise)
                {
                    if (value < 50f)
                    {
                        value += 1f;
                    }
                    else
                    {
                        rise = false;
                    }
                }
                else
                {
                    if (value > 0)
                    {
                        value -= 1f;
                    }
                    else
                    {
                        rise = true;
                    }
                }
                SetColorIntensity(value);
                yield return new WaitForSeconds(0.03f);
            }
        }

        private void SetColorIntensity(float value)
        {
            
            _material?.SetColor("_EmissionColor", _color * value);
        }
    }
}