using System;
using UnityEngine;

namespace ITPRO.Rover.User.Example
{
    public class CodeWriter : MonoBehaviour
    {
        private void OnMouseDown()
        {
            StatisticWriter.instance.StartTime = "01.11.22";
        }
    }   
}