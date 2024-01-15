using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace ITPRO.Rover.User.Example
{
    public class EventWriter : MonoBehaviour
    {
        [SerializeField] private UnityStartTimeEvent startTimeEvent = new UnityStartTimeEvent();

        private void OnMouseDown()
        {
            string currentTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            
            startTimeEvent.Invoke(currentTime);
        }
    }

    [Serializable]
    public class UnityStartTimeEvent : UnityEvent<string> {}   
}