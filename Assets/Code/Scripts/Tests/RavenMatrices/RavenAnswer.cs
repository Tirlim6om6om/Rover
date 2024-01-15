using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.Test.Raven
{
    public class RavenAnswer : MonoBehaviour
    {
        [SerializeField] private int number;
        [SerializeField] private Image answerImage;
        
        public int Number => number;

        public Image AnswerImage => answerImage;
    }   
}