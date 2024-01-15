using System;
using ITPRO.Rover.Controller;
using ITPRO.Rover.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace ITPRO.Rover.Test
{
    public class RDOTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] private TestController controller;
        [SerializeField] private StepChecker stepChecker;
        [SerializeField] private TravelStatistic statistic;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.GetComponent<Collider>().gameObject.layer) & layer) == 0) return;
            controller.ActivateTest(TypeTest.RDO);
            gameObject.SetActive(false);
            statistic.NextPoint();
            stepChecker.NextStep();
        }
    }   
}