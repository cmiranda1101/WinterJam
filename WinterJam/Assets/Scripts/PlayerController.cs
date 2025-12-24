using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;

    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        controls = new PlayerControls();                    //Set up input system
        controller = GetComponent<CharacterController>();   //Get Character Controller
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
    }

    void movePlayer()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();  //Grab the input
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = transform.TransformDirection(move);                  //Transform dir from local to world
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
