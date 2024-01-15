using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.Minerals
{
    public class CollectedMinerals : MonoBehaviour
    {
        private int collected = 0;
        public void AddCollected()
        {
            collected += 1;
            StatisticWriter.instance.CollectionSamples = collected;
        }
    }
}