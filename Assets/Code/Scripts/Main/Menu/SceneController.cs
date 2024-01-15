using System;
using System.Collections;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITPRO.Rover.Managers
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance;
        [SerializeField] private GameObject loadUI;
        [SerializeField] private GameObject inputUI;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        public void LoadLevel(int index)
        {
            SetLoad();
            StartCoroutine(AsyncLoad(index));
        }

        public void Reload()
        {
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetLoad()
        {
            loadUI?.SetActive(true);
            inputUI?.SetActive(false);
            
        }

        public void LoadLevel(User.User _,  GameDifficult difficult)
        {
            LoadLevel((int)difficult + 1);
        }

        private IEnumerator AsyncLoad(int index)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(index);
            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
