using UnityEngine;

public class CreatureMovementHandler : MonoBehaviour
{
    private float _dirX, _speed;

    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _dirX = 1.3f;
        _speed = 1.3f;
    }

    private void Update()
    {
        if (transform.position.x < -10f)
        {
            _dirX = 1.3f;
        }
        else if (transform.position.x > 10)
        {
            _dirX = -1.3f;
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = new Vector2(_dirX * _speed, _rigidBody.linearVelocity.y);
    }
}