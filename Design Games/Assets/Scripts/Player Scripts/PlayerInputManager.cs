using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 movement;
    public static bool moveIsHeld;
    public static bool interactIsPressed;
    public static bool scanIsPressed;

    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _scanAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _scanAction = PlayerInput.actions["ScanCreature"];
        _interactAction = PlayerInput.actions["Interact"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();

        moveIsHeld = _moveAction.IsPressed();
        interactIsPressed = _interactAction.WasPressedThisFrame();
        scanIsPressed = _scanAction.IsPressed();
    }
}