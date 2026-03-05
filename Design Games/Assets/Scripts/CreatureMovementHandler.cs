using UnityEngine;

public class CreatureMovementHandler : MonoBehaviour, ICreature
{
    [SerializeField] private CreatureData _creature;

    private float _dirX, _speed;

    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _dirX = 2f;
        _speed = 2f;
    }

    private void Update()
    {
        if (transform.position.x < -10f)
        {
            _dirX = 2f;
        }
        else if (transform.position.x > 10)
        {
            _dirX = -2f;
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = new Vector2(_dirX * _speed, _rigidBody.linearVelocity.y);
    }

    public CreatureData GetCreatureData()
    {
        return _creature;
    }
}