using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class CreatureAIHandler : MonoBehaviour, ICreature
{
    private enum BehaviourState
    {
        Wander,
        Chase,
        Flee
    }

    [Header("Creature Data")]
    [SerializeField] private CreatureData _creatureData;

    [Header("Creature References")]
    [SerializeField] private TextMeshPro _creatureName;
    [SerializeField] private LayerMask _wallLayer;

    private float _currentHealth;
    private float _lastAttackTime;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Transform _player;

    private Vector2 _moveInput;
    private int _lastHorizontalFacing = 1;
    private float _wanderTimer;
    private float _lastLungeTime;

    private float _lastCollisionTime = -1f;
    [SerializeField] private float _collisionCooldown = 0.1f;

    private BehaviourState _currentState = BehaviourState.Wander;

    private const float WANDER_WEIGHT = 0.5f;
    private const float THREAT_WEIGHT = 2.0f;
    private const float AVOID_WEIGHT = 3.0f;

    public CreatureData GetCreatureData() => _creatureData;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (_creatureData != null) _creatureName.text = _creatureData.name;

        _rigidbody.gravityScale = _creatureData._creatureGravityScale;
        _currentHealth = _creatureData._creatureMaxHealth;
        transform.localScale = Vector3.one * _creatureData._creatureSize;
    }

    private void FixedUpdate()
    {
        Vector2 steering = Vector2.zero;

        steering += WanderBehaviour() * WANDER_WEIGHT;
        steering += HandleThreats() * THREAT_WEIGHT;
        steering += AvoidWalls() * AVOID_WEIGHT;

        _moveInput = steering.normalized;

        UpdateFacing();
        ApplyMovement();
        ApplyTilt();
    }

    private Vector2 WanderBehaviour()
    {
        _wanderTimer -= Time.fixedDeltaTime;

        if (_wanderTimer <= 0f)
        {
            if (Random.value < _creatureData._wanderFlipChance) _lastHorizontalFacing *= -1;

            float horizontal = Random.Range(0.5f, 1f) * _lastHorizontalFacing;
            float vertical = Random.Range(-_creatureData._wanderVerticalRange, _creatureData._wanderVerticalRange);

            _moveInput = new Vector2(horizontal, vertical).normalized;

            _wanderTimer = Random.Range(_creatureData._wanderMinTime, _creatureData._wanderMaxTime);
        }

        return _moveInput;
    }

    private Vector2 HandleThreats()
    {
        Transform closestPrey = null;
        float closestPreyDist = Mathf.Infinity;

        Transform closestPredator = null;
        float closestPredatorDist = Mathf.Infinity;

        var allCreatures = GameObject.FindObjectsByType<CreatureAIHandler>(FindObjectsSortMode.None);

        foreach (var creature in allCreatures)
        {
            if (creature == this) continue;
            if (creature.CompareTag("Scanner")) continue;

            float dist = Vector2.Distance(transform.position, creature.transform.position);

            if (_creatureData._creaturePrey.Contains(creature.GetCreatureData()) && dist < closestPreyDist)
            {
                closestPrey = creature.transform;
                closestPreyDist = dist;
            }

            if (creature.GetCreatureData()._creaturePrey.Contains(_creatureData) && dist < closestPredatorDist)
            {
                closestPredator = creature.transform;
                closestPredatorDist = dist;
            }
        }

        if (closestPredator != null && closestPredatorDist <= _creatureData._creatureFleeRange)
        {
            _currentState = BehaviourState.Flee;
            return (transform.position - closestPredator.position).normalized;
        }

        if (closestPrey != null && closestPreyDist <= _creatureData._creatureDetectionRange)
        {
            _currentState = BehaviourState.Chase;

            Vector2 dir = (closestPrey.position - transform.position).normalized;

            TryLunge(dir);
            TryAttack();

            return dir;
        }

        if (_player != null)
        {
            float playerDist = Vector2.Distance(transform.position, _player.position);

            var transformHandler = TransformationDeviceHandler._instance;

            if (transformHandler != null && transformHandler._currentTransformedCreature != null)
            {
                var playerCreature = transformHandler._currentTransformedCreature;

                if (playerCreature._creaturePrey.Contains(_creatureData))
                {
                    _currentState = BehaviourState.Flee;

                    PlayerMovementHandler._instance.RegisterThreat(
                        _creatureData._creatureFleeSpeed / _creatureData._creatureSwimMaxSpeed
                    );

                    return (transform.position - _player.position).normalized;
                }
            }

            if (playerDist <= _creatureData._creatureDetectionRange && CanAttackPlayer())
            {
                _currentState = BehaviourState.Chase;

                Vector2 dir = (_player.position - transform.position).normalized;

                TryLunge(dir);
                TryAttack();

                return dir;
            }
        }

        _currentState = BehaviourState.Wander;
        return Vector2.zero;
    }

    private Vector2 AvoidWalls()
    {
        float distance = _creatureData._wallDetectionDistance * _creatureData._creatureSize;

        Vector2 forward = new Vector2(_lastHorizontalFacing, 0f);
        Vector2 upForward = (forward + Vector2.up * 0.5f).normalized;
        Vector2 downForward = (forward + Vector2.down * 0.5f).normalized;

        Vector2 avoidance = Vector2.zero;

        avoidance += CheckDirection(forward, distance);
        avoidance += CheckDirection(upForward, distance);
        avoidance += CheckDirection(downForward, distance);

        avoidance += CheckDirection(Vector2.up, distance * 0.8f);
        avoidance += CheckDirection(Vector2.down, distance * 0.8f);

        return avoidance;
    }

    private Vector2 CheckDirection(Vector2 dir, float distance)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = _collider.Cast(dir, hits, distance);

        if (hitCount > 0 && hits[0].collider != null)
        {
            if (hits[0].collider.isTrigger || hits[0].collider.CompareTag("Scanner"))
                return Vector2.zero;

            float proximityFactor = 1f - (hits[0].distance / distance);

            float scaledCooldown = Mathf.Lerp(0f, _collisionCooldown * 2f, proximityFactor);
            if (Time.time - _lastCollisionTime < scaledCooldown)
                return Vector2.zero;

            _lastCollisionTime = Time.time;

            return -dir * proximityFactor;
        }

        return Vector2.zero;
    }

    private void ApplyMovement()
    {
        if (_moveInput == Vector2.zero) return;

        float speed = _creatureData._creatureSwimMaxSpeed;

        if (_currentState == BehaviourState.Flee)
            speed = _creatureData._creatureFleeSpeed;

        Vector2 targetVelocity = _moveInput * speed;
        Vector2 velocityDelta = targetVelocity - _rigidbody.linearVelocity;

        _rigidbody.AddForce(velocityDelta * 0.5f);

        if (_rigidbody.linearVelocity.magnitude > speed)
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * speed;
    }

    private void UpdateFacing()
    {
        if (_moveInput.x > 0.1f) _lastHorizontalFacing = 1;
        else if (_moveInput.x < -0.1f) _lastHorizontalFacing = -1;
    }

    private void ApplyTilt()
    {
        if (_moveInput.magnitude < 0.1f) return;

        float vertical = _moveInput.y;
        float targetAngle = _lastHorizontalFacing == 1 ? -90f : 90f;
        float threshold = _creatureData._verticalTiltThreshold;

        if (vertical > threshold) targetAngle += _lastHorizontalFacing == 1 ? 45f : -45f;
        else if (vertical < -threshold) targetAngle += _lastHorizontalFacing == 1 ? -45f : 45f;

        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, _creatureData._creatureTiltSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void TryAttack()
    {
        Debug.Log("ATTEMPTING ATTACK");

        if (Time.time - _lastAttackTime < _creatureData._creatureAttackCooldown) return;

        _lastAttackTime = Time.time;

        Vector2 center = (Vector2)transform.position + new Vector2(_lastHorizontalFacing, 0f) * _creatureData._biteOffset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, _creatureData._biteRadius);

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            if (hit.CompareTag("Scanner")) continue;

            var creature = hit.GetComponent<CreatureAIHandler>();
            if (creature != null && _creatureData._creaturePrey.Contains(creature.GetCreatureData()))
            {
                creature.TakeDamage(_creatureData._creatureAttackDamage);
                continue;
            }

            if (hit.CompareTag("Player") && CanAttackPlayer())
            {
                HandlePlayerHit(hit.transform);
            }
        }
    }

    private void TryLunge(Vector2 direction)
    {
        if (!_creatureData._canLunge) return;
        if (Time.time - _lastLungeTime < _creatureData._lungeCooldown) return;

        _lastLungeTime = Time.time;

        _rigidbody.AddForce(direction * _creatureData._lungeForce, ForceMode2D.Impulse);
    }

    private void HandlePlayerHit(Transform player)
    {
        var health = player.GetComponent<PlayerHealthHandler>();
        if (health == null) return;

        Vector2 dir = (player.position - transform.position).normalized;

        health.TakeDamage(_creatureData._creatureAttackDamage, dir);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private bool CanAttackPlayer()
    {
        var transformHandler = TransformationDeviceHandler._instance;

        if (transformHandler != null && transformHandler._currentTransformedCreature != null)
        {
            var playerCreature = transformHandler._currentTransformedCreature;
            return _creatureData._creaturePrey.Contains(playerCreature);
        }
        else
        {
            return _creatureData._attacksPlayerBaseForm;
        }
    }

    private void Die()
    {
        // TODO: add particles, score, etc.

        Destroy(gameObject);
    }
}