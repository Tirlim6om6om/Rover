using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ITPRO.Rover.Controller
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TextMeshProUGUI text;
        private float _speed;
        
        private void Start()
        {
            StartCoroutine(CheckSpeed());
        }

        private IEnumerator CheckSpeed()
        {
            while (true)
            {
                _speed = (float)Math.Round(rb.velocity.magnitude * 3.6f);
                text.text =  _speed.ToString("00") + " км/ч";
                yield return new WaitForSeconds(0.1f);
            }
        }

        public float GetSpeed()
        {
            return _speed;
        }
        
        public float GetSpeedMeter()
        {
            return _speed/3.6f;
        }
    }
}
