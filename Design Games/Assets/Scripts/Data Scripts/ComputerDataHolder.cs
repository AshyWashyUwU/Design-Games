using UnityEngine;
using System.Collections.Generic;

public class ComputerDataHolder : MonoBehaviour
{
    private static ComputerDataHolder Instance;
    public static ComputerDataHolder _instance { get => Instance; }

    private Dictionary<CreatureData, float> _storedCreatureData = new Dictionary<CreatureData, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UploadCreatureData(CreatureData _creature, float _amount)
    {
        if (_storedCreatureData.ContainsKey(_creature))
        {
            _storedCreatureData[_creature] = _amount;
        }
        else
        {
            _storedCreatureData.Add(_creature, _amount);
        }
    }

    public float GetCreatureDataAmount(CreatureData _creature)
    {
        if (_storedCreatureData.TryGetValue(_creature, out float _amount)) return _amount;

        return 0f;
    }
}
