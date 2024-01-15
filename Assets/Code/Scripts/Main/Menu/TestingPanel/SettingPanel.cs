using ITPRO.Rover.Managers;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.Events;

public class SettingPanel : MonoBehaviour
{
    public UnityEvent<User, GameDifficult> endSetup = new ();
    
    [SerializeField] private GameObject difficultyButtons;
    [SerializeField] private GameObject selectButtons;
    [SerializeField] private GameObject instruction;
    [SerializeField] private SceneController sceneController;
    
    private User _selectUser;
    private GameDifficult _difficulty;

    private void Awake()
    {
        endSetup.AddListener(delegate(User activeUser, GameDifficult activeDifficult)
        {
            StatisticWriter.ActiveUser = activeUser;
            StatisticWriter.ActiveDifficult = activeDifficult;
        });
    }

    private void OnEnable()
    {
        instruction.SetActive(false);
        difficultyButtons.SetActive(false);
        selectButtons.SetActive(true);
    }

    public void SelectUser(UserPanel userPanel)
    {
        _selectUser = userPanel.PanelUser;
    }
    
    public void ConfirmSelect()
    {
        difficultyButtons.SetActive(true);
        selectButtons.SetActive(false);
    }

    public void CancelSelect()
    {
        gameObject.SetActive(false);
    }

    public void SelectDifficulty(int difficulty)
    {
        _difficulty = (GameDifficult)difficulty;
        instruction.SetActive(true);
        difficultyButtons.SetActive(false);
    }

    public void CloseInstruction()
    {
        gameObject.SetActive(false);
        endSetup.Invoke(_selectUser, _difficulty);
    }
}