using UnityEngine;
using TMPro;

public class CreatureDataHandler : MonoBehaviour, ICreature
{
    [SerializeField] private CreatureData _creature;
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
