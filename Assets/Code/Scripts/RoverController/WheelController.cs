using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ITPRO.Rover
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] protected WheelCollider wheel;
        private float speed;

        protected void FixedUpdate()
        {
            Vector3 position;
            Quaternion rotation;
            wheel.GetWorldPose(out position, out rotation);
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
