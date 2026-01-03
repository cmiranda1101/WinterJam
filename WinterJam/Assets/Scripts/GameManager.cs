using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Stack<GameObject> menueQueue;
    public PlayerControls controls { get; private set; }
    public GameObject player;
    public PlayerController playerScript;
    public GameObject HUD;
    public GameObject pauseMenu;
    public GameObject SettingsMenu;
    public GameObject ControlsMenu;
    public GameObject CreditsMenu;
    public ButtonFunctions buttonFunctions;
    public Image playerHealth;
    public Image playerStamina;
    public bool isPaused;

    public static GameManager instance { get; private set; }
    public static AudioManager audioManager {get; private set;}


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        controls = new PlayerControls();
        menueQueue = new Stack<GameObject>();
        isPaused = false;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // put cursor to center
        Cursor.visible = false;

        HUD.SetActive(true);
        pauseMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    private void Update()
    {
        if(!audioManager.musicSource.isPlaying)
        {
            audioManager.PlayMusic();
        }
    }

    private void OnEnable()
    {
        // Bind to Events
        controls.GameManager.Pause.performed += OnPause;    //Subscribe
        controls.GameManager.Enable();
    }

    private void OnDisable()
    {
        controls.GameManager.Pause.performed -= OnPause;    //Unsubscribe
        controls.GameManager.Disable();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (!playerScript.isDead)
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        playerScript.enabled = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            HUD.SetActive(!isPaused);
            menueQueue.Push(pauseMenu);
            pauseMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HUD.SetActive(!isPaused);
            while(menueQueue.Count > 0)
            {
                menueQueue.Peek().SetActive(false);
                menueQueue.Pop();
            }
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        playerScript = player;
        this.player = player.gameObject;
    }

    public void RegisterAudioManager(AudioManager AM)
    {
        audioManager = AM;
    }
}
