using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class LicenseSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField keyInput;
    [SerializeField] private Button activateButton;

    [SerializeField] private Animator errorMessageAnim;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private GameObject loadingIcon;

    string _directory = "C:\\ITPRO";
    string _licenseFilePath = "C:\\ITPRO\\license.whatislove";

    private void Awake()
    {
        CheckClientLicense();
    }

    public void SendLicense()
    {
        string licenseKey = keyInput.text;
        licenseKey = licenseKey.Trim(' ');
        licenseKey.ToLower();

        if (licenseKey == string.Empty)
        {
            errorText.text = "введите ключ активации";
            keyInput.textComponent.color = Color.red;
            errorMessageAnim.SetTrigger("Error");
            return;
        }

        StartCoroutine(CheckServerLicense(licenseKey, true));
    }

    public void ResetKeyInput()
    {
        keyInput.text = string.Empty;
        keyInput.textComponent.color = Color.white;
    }

    private void ShowErrorMessage(string message)
    {
        errorText.text = message;
        keyInput.textComponent.color = Color.red;
        errorMessageAnim.SetTrigger("Error");
    }
    
    private void CheckClientLicense()
    {
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
            File.Create(_licenseFilePath);
            return;
        }
        if (!File.Exists(_licenseFilePath)) return;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_licenseFilePath, FileMode.Open);
        string license = formatter.Deserialize(stream) as string;
        stream.Close();
        StartCoroutine(CheckServerLicense(license, false));
    }

    private void SaveLicense(string license)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_licenseFilePath, FileMode.OpenOrCreate);
        formatter.Serialize(stream, license);
        stream.Close();
    }

    private IEnumerator CheckServerLicense(string license, bool checkActivation)
    {
        titleText.text = "Проверка лицензии...";

        loadingIcon.SetActive(true);
        activateButton.interactable = false;
        keyInput.interactable = false;

        yield return new WaitForSeconds(2.5f);
        
        if (checkActivation)
        {
            WWWForm form = new();
            form.AddField("license", license);
            using (UnityWebRequest www = UnityWebRequest.Post("https://test109283.unis.pro/licenseAccess.php", form))
            {
                yield return www.SendWebRequest();
                string code = www.downloadHandler.text;
                switch (code)
                {
                    case "400":
                        ShowErrorMessage("срок действия лицензии истек");
                        break;
                    case "402":
                        ShowErrorMessage("количество активаций закончилось");
                        break;
                    case "404":
                        ShowErrorMessage("ключ введен неверно");
                        break;
                    case "200":
                        SaveLicense(license);
                        titleText.text = "Лицензия активирована! Загрузка приложения...";
                        titleText.color = new Color(0, 1, 0.4313726f);
                        StartCoroutine(LoadApp());
                        yield break;
                    default:
                        ShowErrorMessage("неизвестная ошибка");
                        break;
                }
            }
        }
        else
        {
            WWWForm form = new();
            form.AddField("license", license);
            using (UnityWebRequest www = UnityWebRequest.Post("https://test109283.unis.pro/licenseAccessNoActivation.php", form))
            {
                yield return www.SendWebRequest();
                string code = www.downloadHandler.text;
                switch (code)
                {
                    case "400":
                        ShowErrorMessage("срок действия лицензии истек");
                        break;
                    case "404":
                        ShowErrorMessage("ключ введен неверно");
                        break;
                    case "200":
                        SaveLicense(license);
                        titleText.text = "Лицензия активирована! Загрузка приложения...";
                        titleText.color = new Color(0, 1, 0.4313726f);
                        StartCoroutine(LoadApp());
                        yield break;
                    default:
                        ShowErrorMessage("неизвестная ошибка");
                        break;
                }
            }
        }
        
        titleText.text = "Введите ключ активации:";
        keyInput.interactable = true;
        activateButton.interactable = true;
        loadingIcon.SetActive(false);
    }

    private IEnumerator LoadApp()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync(1);
    }
}
