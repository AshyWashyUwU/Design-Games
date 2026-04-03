using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureSlotHolder : MonoBehaviour
{
    private static CreatureSlotHolder Instance;
    public static CreatureSlotHolder _instance { get => Instance; }

    public List<CreatureData> _creatureSlots = new List<CreatureData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _creatureSlots = new List<CreatureData>(new CreatureData[6]);
    }

    public void ChangeCreatureSlot(CreatureData creature, int slotIndex)
    {
        if (_creatureSlots[slotIndex] == creature)
        {
            _creatureSlots[slotIndex] = null; return;
        }

        for (int i = 0; i < _creatureSlots.Count; i++)
        {
            if (_creatureSlots[i] == creature)
            {
                _creatureSlots[i] = null;
            }
        }

        _creatureSlots[slotIndex] = creature;
    }

    public List<CreatureData> GetCreatureSlotData()
    {
        return _creatureSlots;
    }
}
