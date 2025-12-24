using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Camera playerCam;
    private float pitch;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float minPitchAngle;
    [SerializeField] private float maxPitchAngle;

    private void Awake()
    {
        controls = new PlayerControls();                    // Set up input system
        controller = GetComponent<CharacterController>();   // Get Character Controller
        playerCam = GetComponentInChildren<Camera>();       // Get Camera

        pitch = 0.0f;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
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
}
