using UnityEngine;
using System.Collections.Generic;

public class ComputerDataHolder : MonoBehaviour
{
    private static ComputerDataHolder Instance;
    public static ComputerDataHolder _instance { get => Instance; }

    private Dictionary<CreatureData, float> _storedCreatureData = new Dictionary<CreatureData, float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UploadCreatureData(CreatureData creature, float amount)
    {
        if (_storedCreatureData.ContainsKey(creature))
        {
            _storedCreatureData[creature] = amount;
        }
        else
        {
            _storedCreatureData.Add(creature, amount);
        }
    }

    public float GetCreatureDataAmount(CreatureData creature)
    {
        if (_storedCreatureData.TryGetValue(creature, out float amount)) return amount;

        return 0f;
    }
}
