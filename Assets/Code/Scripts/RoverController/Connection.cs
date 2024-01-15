using System;
using System.Collections;
using ITPRO.Rover.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.Controller
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform roverPos;
        [SerializeField] private Image connectionBar;
        [SerializeField] private TextMeshProUGUI connectionText;
        [SerializeField] private float distanceMax;

        private void Start()
        {
            StartCoroutine(CheckConnection());
        }

        private IEnumerator CheckConnection()
        {
            while (true)
            {
                float distance = 1 - Vector3.Distance(startPos.position, roverPos.position) / distanceMax;
                connectionBar.fillAmount = distance;
                connectionText.SetText(Math.Round(distance*100) + "%");
                if (distance <= 0)
                {
                    GameManager.instance.Lose("выезд за пределы радиосигнала");
                    break;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
