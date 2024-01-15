using System;
using UnityEngine;
using ITPRO.Rover.Controller;
using ITPRO.Rover.Managers;

namespace ITPRO.Rover.Junction
{
    public class JunctionNode : MonoBehaviour
    {
        [SerializeField] private GameObject roverConnection;
        [SerializeField] private RoverController controller;
        [SerializeField] private Transform pointConnect;
        [SerializeField] private GameObject collider;
        [SerializeField] private float maxAngle;
        [SerializeField] private float maxDistance;

        private bool _active;

        private void Start()
        {
            Lock(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            float angle = Math.Abs(controller.transform.eulerAngles.y - transform.eulerAngles.y);
            if (other.gameObject == roverConnection && (angle < maxAngle || angle > 360 - maxAngle))
            {
                Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(pointConnect.position);
                pos = new Vector3(pos.x, pos.y, 0);
                if (Vector3.Distance(Vector3.zero, pos) < maxDistance)
                {
                    Lock(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == roverConnection)
            {
                Lock(false);
                StepChecker.instance.startSteps = true;
            }
        }

        public void Lock(bool locking)
        {
            _active = locking;
            collider.SetActive(!locking);
            controller.SetLock(locking);
        }

        public bool GetActive()
        {
            return _active;
        }
    }
}
