using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 movement;
    public static bool moveIsHeld;
    public static bool scanIsPressed;

    private InputAction _moveAction;
    private InputAction _scanAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _scanAction = PlayerInput.actions["ScanCreature"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();

        scanIsPressed = _scanAction.IsPressed();
        moveIsHeld = _moveAction.IsPressed();
    }
}