using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Objects/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("Creature Stats")]
    public float _creatureSwimMaxSpeed = 3;
    public float _creatureSwimForce = 3;
    public float _creatureGravityScale = 0.025f;
    public float _creatureMaxTiltAngle = 15;
    public float _creatureTiltSpeed = 5;

    public float _creatureDetectionRange = 10f;
    public float _creatureFleeRange = 5f;
    public float _creatureFleeSpeed = 3f;

    [Header("Basic Creature Data")]
    public float _creatureDataMaximum;
    public CreatureType _creatureType;
    public string _creatureDescription;
    public Sprite _creatureSprite;
    public float _creatureSize = 1f;

    [Header("Creature Combat")]
    public float _creatureMaxHealth = 10f;
    public float _creatureAttackDamage = 5f;
    public float _creatureAttackRange = 2f;
    public float _creatureAttackCooldown = 0.5f;

    [Header("Bite Settings")]
    public float _biteRadius = 0.5f;
    public float _biteOffset = 0.5f;

    [Header("Lunge")]
    public bool _canLunge;
    public float _lungeForce = 10f;
    public float _lungeCooldown = 2f;

    [Header("AI Behaviour Settings")]
    public float _wanderMinTime = 1f;
    public float _wanderMaxTime = 3f;

    [Range(0f, 1f)] public float _wanderFlipChance = 0.1f;

    public float _wanderVerticalRange = 0.2f;
    public float _verticalTiltThreshold = 0.2f;
    public float _wallDetectionDistance = 2f;

    [Header("Advanced Creature Data")]
    public List<CreatureData> _creaturePrey;
    public List<int> _creatureLayers;

    public bool _fearsPlayerBaseForm;
    public bool _attacksPlayerBaseForm;
}

public enum CreatureType
{
    _producer,
    _primaryConsumer,
    _secondaryConsumer,
    _apexPredator,
    _unclassified
}