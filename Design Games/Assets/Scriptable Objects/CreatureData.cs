using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Objects/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("Basic Creature Data")]
    public float _researchRequired;
    public CreatureType _creatureType;

    [Header("Advanced Creature Data")]
    public List<CreatureData> _creaturePrey;
}

public enum CreatureType
{
    _producer,
    _primaryConsumer,
    _secondaryConsumer,
    _apexPredator,
    _unclassified
}
