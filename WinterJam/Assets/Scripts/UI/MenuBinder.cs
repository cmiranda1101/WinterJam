using UnityEngine;
using UnityEngine.UI;

public class MenuBinder : MonoBehaviour
{
    public Button resumeButton;
    public Button settingsButton;
    public Button backButton;
    public Button exitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onResume());
        settingsButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onSettings());
        backButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onBack());
        exitButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onExit());
    }
}
