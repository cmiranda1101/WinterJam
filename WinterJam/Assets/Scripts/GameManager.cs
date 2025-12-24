using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerControls controls;

    public GameObject player;
    public PlayerController playerScript;

    public bool isPaused;


    private void Awake()
    {
        controls = new PlayerControls();

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        isPaused = false;

        // Bind to Events
        controls.GameManager.Pause.performed += _ => TogglePause(); // _ is lambda for I know this param
                                                                    // exist but I don't need it. Binds
                                                                    // the func to the event of Pause
    }

    private void OnEnable()
    {
        controls.GameManager.Enable();
    }

    private void OnDisable()
    {
        controls.GameManager.Disable();
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        playerScript.enabled = !isPaused;
    }
}
