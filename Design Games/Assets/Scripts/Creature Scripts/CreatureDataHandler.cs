using UnityEngine;
using TMPro;

public class CreatureDataHandler : MonoBehaviour, ICreature
{
    [Header("Creature Stats")]
    [SerializeField] private CreatureData _creature;

    [Header("Temp References")]
    [SerializeField] private TextMeshPro _creatureName;

    private void Awake()
    {
        if (_creature != null)
        {
            _creatureName.text = _creature.name;
        }
    }

    public CreatureData GetCreatureData()
    {
        return _creature;
    }
}
