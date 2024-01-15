using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private string password;
    [SerializeField] private TMP_Text errorText;

    public UnityEvent authorizationPassed = new UnityEvent();

    private void OnEnable()
    {
        passwordField.text = string.Empty;
        errorText.text = string.Empty;
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void CheckPassword()
    {
        if (password != passwordField.text)
        {
            StartCoroutine(ErrorText());
            return;
        }
        
        gameObject.SetActive(false);
        authorizationPassed.Invoke();
    }

    private IEnumerator ErrorText()
    {
        errorText.text = "Неверный пароль!";
        yield return new WaitForSeconds(4);
        errorText.text = string.Empty;
    }
}