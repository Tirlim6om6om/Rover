using System.Collections;
using TMPro;
using UnityEngine;

namespace ITPRO.Rover.UI
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI text;

        public static Notification instance;

        private void Start()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void SetNotification(string str, float time = 5)
        {
            text.SetText(str);
            anim.SetBool("Open", true);
            StartCoroutine(CloseNotification(time));
        }

        private IEnumerator CloseNotification(float time)
        {
            yield return new WaitForSeconds(time);
            anim.SetBool("Open", false);
        }
    }
}