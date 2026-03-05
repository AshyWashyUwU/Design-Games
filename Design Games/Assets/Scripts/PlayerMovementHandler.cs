using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Walking Variables")]
    [SerializeField] private float _maxWalkSpeed;
    [SerializeField] private float _walkAcceleration;
    [SerializeField] private float _walkDeceleration;
    [SerializeField] private float _walkingGravityScale;

    [Header("Swimming Variables")]
    [SerializeField] private float _maxSwimSpeed;
    [SerializeField] private float _swimMoveForce;
    [SerializeField] private float _swimmingGravityScale;

    private Vector2 _moveVelocity;

    private bool _isFacingRight, _isSwimming;

    private Rigidbody2D _playerRigidbody;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        TogglePlayerMovementType();
    }

    public void TogglePlayerMovementType()
    {
        _isSwimming = !_isSwimming;

        if (_isSwimming)
        {
            _playerRigidbody.gravityScale = _swimmingGravityScale;
        }
        else
        {
            _playerRigidbody.gravityScale = _walkingGravityScale;
        }
    }

    private void FixedUpdate()
    {
        Vector2 _input = PlayerInputManager.movement;

        if (_isSwimming)
        {
            HandleSwimmingMovement(_input);
        }
        else
        {
            float _horizontal = Mathf.Sign(_input.x);
            if (_input.x == 0) _horizontal = 0;

            HandleGroundMovement(_horizontal);
        }
    }

    private void HandleGroundMovement(float _horizontal)
    {
        float _targetSpeed = _horizontal * _maxWalkSpeed;
        float _newX = Mathf.Lerp(_playerRigidbody.linearVelocity.x, _targetSpeed, (_horizontal != 0 ? _walkAcceleration : _walkDeceleration) * Time.fixedDeltaTime);

        float _newY = _playerRigidbody.linearVelocity.y;
        if (_newY > 0f) _newY = 0f;

        _playerRigidbody.linearVelocity = new Vector2(_newX, _newY);
    }

    private void HandleSwimmingMovement(Vector2 _input)
    {
        if (_input != Vector2.zero)
        {
            _playerRigidbody.AddForce(_input.normalized * _swimMoveForce);
        }

        if (_playerRigidbody.linearVelocity.magnitude > _maxSwimSpeed)
        {
            _playerRigidbody.linearVelocity = _playerRigidbody.linearVelocity.normalized * _maxSwimSpeed;
        }
    }

    private void FlipPlayer()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 _scale = transform.localScale;

        if (_isFacingRight)
        {
            _scale.x = Mathf.Abs(_scale.x) * -1;
        }
        else
        {
            _scale.x = Mathf.Abs(_scale.x);
        }

        transform.localScale = _scale;
    }
}