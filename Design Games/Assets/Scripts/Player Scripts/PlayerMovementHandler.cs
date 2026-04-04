using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    private static PlayerMovementHandler Instance;
    public static PlayerMovementHandler _instance { get => Instance; }

    private float _maxWalkSpeed = 5;
    private float _walkAcceleration = 25;
    private float _walkDeceleration = 5;
    private float _walkingGravityScale = 1;

    private float _maxSwimSpeed = 3;
    private float _swimMoveForce = 3;
    private float _swimmingGravityScale = 0.025f;

    private float _maxTiltAngle = 15;
    private float _tiltSpeed = 5;

    private Vector2 _moveVelocity;

    private bool _isFacingRight;

    public bool _isSwimming { get; private set; }

    public bool _immobilized { get; private set; }

    private Rigidbody2D _playerRigidbody;

    private int _lastHorizontalFacing = 1; // 1 = right, -1 = left

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void ToggleImmobilization()
    {
        _immobilized = !_immobilized;
    }

    public void UpdateSwimSpeed(float _newSwimSpeed)
    {
        _maxSwimSpeed = _newSwimSpeed;
        _swimMoveForce = _newSwimSpeed;
    }

    public void TogglePlayerMovementType()
    {
        _isSwimming = !_isSwimming;

        if (_isSwimming)
        {
            if (TransformationDeviceHandler._instance._currentTransformedCreature == null)
            {
                _playerRigidbody.gravityScale = _swimmingGravityScale;
            }
            else
            {
                _playerRigidbody.gravityScale = TransformationDeviceHandler._instance._currentTransformedCreature._creatureGravityScale;
            }
        }
        else
        {
            _playerRigidbody.gravityScale = _walkingGravityScale;
        }
    }

    private void FixedUpdate()
    {
        ApplyTilt();

        if (_immobilized) 
        { 
            _playerRigidbody.linearVelocity = new Vector2(0, 0);

            return;
        }

        Vector2 _input = PlayerInputManager._movement;

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
        if (_immobilized) 
        { 
            _playerRigidbody.linearVelocity = new Vector2(0, 0);

            return;
        }

        float _targetSpeed = _horizontal * _maxWalkSpeed;
        float _newX = Mathf.Lerp(_playerRigidbody.linearVelocity.x, _targetSpeed, (_horizontal != 0 ? _walkAcceleration : _walkDeceleration) * Time.fixedDeltaTime);

        float _newY = _playerRigidbody.linearVelocity.y;
        if (_newY > 0f) _newY = 0f;

        _playerRigidbody.linearVelocity = new Vector2(_newX, _newY);
    }

    private void HandleSwimmingMovement(Vector2 _input)
    {
        float _swimForce;
        float _maxSpeed;

        if (TransformationDeviceHandler._instance._currentTransformedCreature == null)
        {
            _swimForce = _swimMoveForce;
            _maxSpeed = _maxSwimSpeed;
        }
        else
        {
            _swimForce = TransformationDeviceHandler._instance._currentTransformedCreature._creatureSwimForce;
            _maxSpeed = TransformationDeviceHandler._instance._currentTransformedCreature._creatureSwimMaxSpeed;
        }

        if (_input != Vector2.zero)
        {
            _playerRigidbody.AddForce(_input.normalized * _swimMoveForce);
        }

        if (_playerRigidbody.linearVelocity.magnitude > _maxSpeed)
        {
            _playerRigidbody.linearVelocity = _playerRigidbody.linearVelocity.normalized * _maxSpeed;
        }
    }

    private void ApplyTilt()
    {
        float targetAngle = 0f;
        var transformedCreature = TransformationDeviceHandler._instance._currentTransformedCreature;

        float tiltAngle;
        float tiltSpeed;

        if (transformedCreature == null || !_isSwimming)
        {
            tiltAngle = _maxTiltAngle;
            tiltSpeed = _tiltSpeed;
        }
        else
        {
            tiltAngle = transformedCreature._creatureMaxTiltAngle;
            tiltSpeed = transformedCreature._creatureTiltSpeed;
        }

        Vector2 input = PlayerInputManager._movement;

        if (transformedCreature != null)
        {
            if (input.magnitude > 0.1f)
            {
                float horizontal = input.x;
                float vertical = input.y;

                if (horizontal > 0.1f) _lastHorizontalFacing = 1;
                else if (horizontal < -0.1f) _lastHorizontalFacing = -1;

                targetAngle = _lastHorizontalFacing == 1 ? -90f : 90f;

                if (vertical > 0.1f)
                {
                    targetAngle += _lastHorizontalFacing == 1 ? 45f : -45f; // moving up
                }
                else if (vertical < -0.1f)
                {
                    targetAngle += _lastHorizontalFacing == 1 ? -45f : 45f; // moving down
                }
            }
            else
            {
                targetAngle = _lastHorizontalFacing == 1 ? -90f : 90f;
            }
        }
        else if (_isSwimming)
        {
            if (input.magnitude > 0.1f)
            {
                float horizontalTilt = -input.x * tiltAngle;
                float verticalTilt = input.y * (tiltAngle * 0.5f);
                targetAngle = horizontalTilt + verticalTilt;
            }
        }
        else
        {
            float horizontalSpeed = _playerRigidbody.linearVelocity.x;
            targetAngle = -horizontalSpeed * tiltAngle / _maxWalkSpeed;
        }

        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, tiltSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
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

    public Transform GetPlayerTransform()
    {
        return transform;
    }
}