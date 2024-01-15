using System;
using ITPRO.Rover.Test.RDO;
using UnityEngine;

namespace ITPRO.Rover.UI
{
    public class NotificationPointMinerals : MonoBehaviour
    {
        [SerializeField] private RDOTest test;
        [SerializeField] private string text;
        
        private void Start()
        {
            test.OnTestEnd += SendNotification;
        }

        public void SendNotification()
        {
            Notification.instance.SetNotification(text);
            test.OnTestEnd -= SendNotification;
            Destroy(gameObject);
        }
    }
}