using UnityEngine;
using System.Collections.Generic;

public class CreatureDataHolder : MonoBehaviour
{
    private static CreatureDataHolder Instance;
    public static CreatureDataHolder _instance { get => Instance; }

    [SerializeField] private List<CreatureData> _creatureList = new List<CreatureData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<CreatureData> GetCreatureList()
    {
        return _creatureList;
    }
}
