using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 movement;
    public static bool moveIsHeld;
    public static bool interactIsPressed;

    private InputAction _moveAction;
    private InputAction _interactAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _interactAction = PlayerInput.actions["Interact"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();

        interactIsPressed = _interactAction.WasPressedThisFrame();
        moveIsHeld = _moveAction.IsPressed();
    }
}