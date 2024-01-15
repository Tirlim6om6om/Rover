using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditField : MonoBehaviour
{
    [SerializeField] private TMP_InputField parameterField;
    [SerializeField] private GameObject errorText;

    public string Value { get; private set; } = string.Empty;

    public TMP_InputField ParameterField => parameterField;

    private void OnDisable()
    {
        Value = string.Empty;
        errorText.SetActive(false);
        ParameterField.text = string.Empty;
    }

    public bool SaveParameter()
    {
        if (ErrorCheck())
        {
            StartCoroutine(ErrorView());
            return false;
        }

        Value = ParameterField.text;
        return true;
    }

    public void InitialValue(string value)
    {
        if (Value == string.Empty) return;

        Value = value;
    }
    
    protected virtual bool ErrorCheck()
    {
        return ParameterField.text == string.Empty;
    }

    private IEnumerator ErrorView()
    {
        errorText.SetActive(true);
        yield return new WaitUntil(() => !ErrorCheck());
        errorText.SetActive(false);
    }

}