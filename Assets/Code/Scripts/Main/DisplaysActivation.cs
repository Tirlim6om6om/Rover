using UnityEngine;


namespace ITPRO.Rover.Displays
{
    public class DisplaysActivation : MonoBehaviour
    {
        void Start ()
        {
            for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }
        }
    }
}
