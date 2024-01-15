using UnityEngine;

namespace ITPRO.Rover.Test.Example
{
    public class ActivateTest : MonoBehaviour
    {
        [SerializeField] private TestBase test;

        public void Activate()
        {
            test.Activate();
        }
    }   
}