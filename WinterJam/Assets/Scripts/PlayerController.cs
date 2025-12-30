using System.Collections;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    private PlayerControls controls;
    private CharacterController controller;
    private Camera playerCam;
    private float pitch;
    private float targetHealthRatio;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float minPitchAngle;
    [SerializeField] private float maxPitchAngle;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthUpdateSpeed;

    [ReadOnly] public float health;
    public bool isDead { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   // Get Character Controller
        playerCam = GetComponentInChildren<Camera>();       // Get Camera

        pitch = 0.0f;
    }

    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);

        targetHealthRatio = 1.0f;
        health = maxHealth;
        isDead = false;
    }

    private void OnEnable() // Gaurds against Null Ref other edge cases I hit
    {
        if (GameManager.instance == null)
            return;

        if (controls == null)
            controls = GameManager.instance.controls;

        if (controls != null)
            controls.Player.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)   // Stops Null Ref
            controls.Player.Disable();
    }
    private void Update()
    {
        if (controls == null)   // This prevents null reference if player is compiled befor GM
        {
            OnDisable();
            OnEnable();
            return;
        }

        HandleDamage();
        movePlayer();
        Look();
    }

    void movePlayer()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();  // Grab move input
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = transform.TransformDirection(move);                  // Transform dir from local to world //Relative to look pos
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        Vector2 input = controls.Player.Look.ReadValue<Vector2>();  // Grab look input
        // Yaw
        transform.Rotate(Vector3.up, input.x * turnSpeed * Time.deltaTime);
        // Pitch
        pitch -= input.y * turnSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitchAngle, maxPitchAngle);
        playerCam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        targetHealthRatio = health / maxHealth;

        if (health == 0)
        {
            GameManager.instance.HUD.SetActive(false);
            GameManager.instance.isDead = true;
            controls.Player.Disable();
            isDead = true;
        }
    }

    void HandleDamage()
    {
        float currRatio = GameManager.instance.playerHealth.fillAmount;
        if (Mathf.Approximately(currRatio, targetHealthRatio)) return;

        GameManager.instance.playerHealth.fillAmount = Mathf.Lerp(currRatio, targetHealthRatio, Time.deltaTime * healthUpdateSpeed);
    }

}
