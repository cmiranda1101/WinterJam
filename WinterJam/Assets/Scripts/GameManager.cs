using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public PlayerControls controls { get; private set; }
    public GameObject player;
    public PlayerController playerScript;
    public GameObject HUD;
    public Image playerHealth;
    public Image playerStamina;
    public bool isPaused;

    public static GameManager instance { get; private set; }

    

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
        isPaused = false;

       
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;  // put cursor to center
        Cursor.visible = false;
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
        TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        playerScript.enabled = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //pauseMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //pauseMenu.SetActive(false);
        }
    }
}
