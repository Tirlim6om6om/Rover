using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ITPRO.Rover.Test.Raven
{
    public class AnswerPosition : MonoBehaviour
    {
        [SerializeField] private int[] numberAnswers;
        [SerializeField] private Image visualizeImage;

        public void VisualizeAnswer(Sprite sprite)
        {
            gameObject.SetActive(true);
            visualizeImage.sprite = sprite;
        }

        public void HideAnswer()
        {
            gameObject.SetActive(false);
            visualizeImage.sprite = null;
        }

        public bool RightPosition(int number)
        {
            foreach (int item in numberAnswers)
            {
                if (number == item)
                {
                    return true;
                }
            }

            return false;
        }
    }   
}