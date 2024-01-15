using System;
using Unity.Mathematics;
using UnityEngine;

namespace ITPRO.Rover.Controller
{
    public class Gizma : MonoBehaviour
    {
        public Vector2 rot;
        [SerializeField] private Transform rover;
        [SerializeField] private Transform gizma;
        [SerializeField] private Transform rotaterZUI;

        private void FixedUpdate()
        {
            rot = new Vector2(rover.eulerAngles.x, rover.eulerAngles.z);
            gizma.localEulerAngles = new Vector3(rot.x, 0, rot.y);
            rotaterZUI.localEulerAngles = new Vector3(0, 0, rot.y);
        }

        public float GetPitch()
        {
            float rot = rover.eulerAngles.x;
            if (rot > 330)
            {
                rot = rot - 360;
            }
            return Mathf.Round(rot);
        }

        public float GetRoll()
        {
            float rot = rover.eulerAngles.z;
            if (rot > 330)
            {
                rot = rot - 360;
            }
            return Mathf.Round(rot);
        }
    }
}