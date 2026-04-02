using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 _movement;
    public static bool _moveIsHeld;
    public static bool _interactIsPressed;
    public static bool _scanIsPressed;

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
        _movement = _moveAction.ReadValue<Vector2>();

        _moveIsHeld = _moveAction.IsPressed();
        _interactIsPressed = _interactAction.WasPressedThisFrame();
        _scanIsPressed = _scanAction.IsPressed();
    }
}