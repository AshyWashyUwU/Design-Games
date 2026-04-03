using UnityEngine;
using System.Collections.Generic;

public class CreatureHotbarHandler : MonoBehaviour
{
    [SerializeField] private List<CreatureHotbarButtonHandler> _creatureButtons = new List<CreatureHotbarButtonHandler>();

    private void OnEnable()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        List<CreatureData> _creatureSlotList = CreatureSlotHolder._instance.GetCreatureSlotData();

        if (_creatureSlotList.Count == _creatureButtons.Count)
        {
            for (int i = 0; i < _creatureSlotList.Count; i++)
            {
                _creatureButtons[i].ChangeCreature(_creatureSlotList[i]);
            }
        }
    }
}
