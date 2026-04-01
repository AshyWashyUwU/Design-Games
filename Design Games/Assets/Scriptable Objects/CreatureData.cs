using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Objects/CreatureData")]
public class CreatureData : ScriptableObject
{
    [Header("Basic Creature Data")]
    public float _creatureDataMaximum;
    public CreatureType _creatureType;
    public string _creatureDescription;
    public Sprite _creatureSprite;

    [Header("Advanced Creature Data")]
    public List<CreatureData> _creaturePrey;
    public List<int> _creatureLayers;
}

public enum CreatureType
{
    _producer,
    _primaryConsumer,
    _secondaryConsumer,
    _apexPredator,
    _unclassified
}