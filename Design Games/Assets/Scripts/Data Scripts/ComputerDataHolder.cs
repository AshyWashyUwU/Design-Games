using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ComputerDataHolder : MonoBehaviour
{
    private static ComputerDataHolder Instance;
    public static ComputerDataHolder _instance { get => Instance; }

    [SerializeField] private TextMeshProUGUI _uploadedResearchTextEncy;
    [SerializeField] private TextMeshProUGUI _uploadedResearchText;

    public float _totalUploadedData;

    private Dictionary<CreatureData, float> _storedCreatureData = new Dictionary<CreatureData, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        _uploadedResearchText.text = "Uploaded Research: " + _totalUploadedData.ToString("F0") + " GB";
        _uploadedResearchTextEncy.text = "Uploaded Research: " + _totalUploadedData.ToString("F0") + " GB";
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

        _totalUploadedData += _amount;
    }

    public float GetCreatureDataAmount(CreatureData _creature)
    {
        if (_storedCreatureData.TryGetValue(_creature, out float _amount)) return _amount;

        return 0f;
    }
}
