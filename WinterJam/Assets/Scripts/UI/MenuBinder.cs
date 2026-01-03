using UnityEngine;
using UnityEngine.UI;

public class MenuBinder : MonoBehaviour
{
    public Button resumeButton;
    public Button settingsButton;
    public Button controlsButton;
    public Button exitButton;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button[] backButtons;

    private void Start()
    {
        // Bind all UI
        resumeButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onResume());
        settingsButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onSettings());
        controlsButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onControls());
        exitButton.onClick.AddListener(() => GameManager.instance.buttonFunctions.onExit());

        musicSlider.onValueChanged.AddListener((float value) => GameManager.audioManager.SetMusicVolume(value));
        sfxSlider.onValueChanged.AddListener((float value) => GameManager.audioManager.SetSFXVolume(value));

        foreach(Button btn in backButtons)
            btn.onClick.AddListener(GameManager.instance.buttonFunctions.onBack);


        // initialize slider values
        musicSlider.value = GameManager.audioManager.musicVolume;
        sfxSlider.value = GameManager.audioManager.sfxVolume;
    }
}
