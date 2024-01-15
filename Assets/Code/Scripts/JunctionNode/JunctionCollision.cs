using System;
using ITPRO.Rover.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace ITPRO.Rover.Junction
{
    public class JunctionCollision : MonoBehaviour
    {
        [SerializeField] private GameObject rover;
        [SerializeField] private Transform posConnect;
        [SerializeField] private Transform pointConnect;
        [SerializeField] private JunctionNode node;
        [SerializeField] private RoverController controller;
        [SerializeField] private Beacon beacon;
        [Serializable] public class TriggerEvent : UnityEvent { }
        [SerializeField] private TriggerEvent connnect = new TriggerEvent();
        [SerializeField] private TriggerEvent disconnect = new TriggerEvent();

        private void Start()
        {
            controller.SetConnect(posConnect);
            beacon.SetActive(true);
        }

        private void OnEnable()
        {
            controller.OutJunction += TurnOffBeacon;
        }

        private void OnDisable()
        {
            controller.OutJunction -= TurnOffBeacon;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == rover)
            {
                if (node.GetActive() && Vector3.Distance(transform.position, pointConnect.position) < 0.05f)
                {
                    controller.SetConnect(posConnect);
                    beacon.SetActive(true);
                    connnect.Invoke();
                }
            }
        }

        public void TurnOffBeacon()
        {
            beacon.SetActive(false);
            disconnect.Invoke();
        }
    }
}