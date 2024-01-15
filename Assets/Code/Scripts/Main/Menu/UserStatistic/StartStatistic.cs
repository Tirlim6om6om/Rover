using System;
using UnityEngine;

namespace ITPRO.Rover.User
{
    public class StartStatistic : MonoBehaviour
    {
        [SerializeField] private GameDifficult difficult;
        private void Start()
        {
            StatisticWriter.instance.StartTime = System.DateTime.Now.ToShortTimeString();
            StatisticWriter.instance.Difficult = difficult;
        }
    }
}
