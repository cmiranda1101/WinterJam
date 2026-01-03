using System.Collections;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    private PlayerControls controls;
    private CharacterController controller;
    private Camera playerCam;
    private Animator playerAnim;
    private float pitch;
    private float targetHealthRatio;
    private IceEffect iceEffectScript;

    [SerializeField] private float animTransSmoothness; // Bigger is smoother
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float minPitchAngle;
    [SerializeField] private float maxPitchAngle;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthUpdateSpeed;
    [SerializeField] private AudioSource playerWalkSource;
    [SerializeField] private AudioSource playerVocalSource;
    [SerializeField] private AudioSource playerOtherSource;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private LayerMask groundMask;

    [ReadOnly] public float health;
    public bool isDead { get; private set; }
    private bool leftDown;
    private bool rightDown;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   // Get Character Controller
        playerCam = GetComponentInChildren<Camera>();       // Get Camera
        playerAnim = GetComponent<Animator>();
        iceEffectScript = GetComponent<IceEffect>();

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
        PlayWalkSound();
    }

    void movePlayer()
    {
        // Player Movement
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();  // Grab move input
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = transform.TransformDirection(move);                  // Transform dir from local to world //Relative to look pos
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Set Anim Parameters
        Vector3 animMove = transform.InverseTransformDirection(move); // Bring back to local for anims
        playerAnim.SetFloat("MoveX", animMove.x, animTransSmoothness, Time.deltaTime);
        playerAnim.SetFloat("MoveY", animMove.z, animTransSmoothness, Time.deltaTime);
        // Speed Param
        float speed = Mathf.Clamp01(animMove.magnitude);
        playerAnim.SetFloat("Speed", speed, animTransSmoothness, Time.deltaTime);
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
            HandleDeath();
        }
    }

    void HandleDamage()
    {
        float currRatio = GameManager.instance.playerHealth.fillAmount;
        if (Mathf.Approximately(currRatio, targetHealthRatio)) return;

        GameManager.instance.playerHealth.fillAmount = Mathf.Lerp(currRatio, targetHealthRatio, Time.deltaTime * healthUpdateSpeed);
    }

    void HandleDeath()
    {
        GameManager.instance.HUD.SetActive(false);
        controls.Player.Disable();
        isDead = true;
        iceEffectScript.SpawnIceBlock();
        // Set Anim Param dead
    }

    // Audio Functions
    void PlayWalkSound()
    {
        CheckFootStep(leftFoot, ref leftDown);
        CheckFootStep(rightFoot, ref rightDown);
    }

    void PlayHurtSound()
    {
        GameManager.audioManager.PlayHurt(playerVocalSource);
    }

    void PlayMeleeSound()
    {
        GameManager.audioManager.PlayMelee(playerVocalSource);
    }

    void PlayEffortSound()
    {
        GameManager.audioManager.PlayAcrobatics(playerVocalSource);
    }

    void CheckFootStep(Transform foot, ref bool footDown)
    {
        bool isDown = Physics.Raycast(foot.position, Vector3.down, 0.2f, groundMask);

        if (isDown && !footDown) // If foot is down and it wasn't prior. Stops duplicat sound on frames
            GameManager.audioManager.PlayMovement(playerWalkSource);
        footDown = isDown;
    }
}