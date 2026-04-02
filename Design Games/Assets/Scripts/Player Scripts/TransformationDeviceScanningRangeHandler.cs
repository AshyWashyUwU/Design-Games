using UnityEngine;
using System.Collections.Generic;

public class TransformationDeviceScanningHandler : MonoBehaviour
{
    private static TransformationDeviceScanningHandler Instance;
    public static TransformationDeviceScanningHandler _instance { get => Instance; }

    [Header("Toggable Variables")]
    [SerializeField] private Color _scanningColor;
    [SerializeField] private Color _normalColor;

    private float _dataMaximum = 100f;
    private float _dataScanRange = 7f;
    private float _totalDataAmount = 0f;
    private float _dataAddAmount = 0.01f;

    private SpriteRenderer _spriteRenderer;

    private Dictionary<CreatureData, float> _storedCreatures = new Dictionary<CreatureData, float>();
    private CreatureData _creatureInRange;

    private Collider2D _collider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void UpdateDataCollectionAmount(float _newCollectionAmount)
    {
        _dataAddAmount = _newCollectionAmount;
    }

    public void UpdateDataMaximum(float _newDataMaximum)
    {
        _storedCreatures.Clear();
        _totalDataAmount = 0f;

        TransformationUIDataHandler._instance.ClearUI();

        _dataMaximum = _newDataMaximum;
    }

    public void UpdateDataScanRange(float _newScanRange)
    {
        _dataScanRange = _newScanRange;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        ICreature _creatureComponent = _collision.GetComponent<ICreature>();

        if (_creatureInRange != null) return;

        if (_creatureComponent != null) _creatureInRange = _creatureComponent.GetCreatureData();
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        ICreature creatureComponent = _collision.GetComponent<ICreature>();
        if (creatureComponent != null && creatureComponent.GetCreatureData() == _creatureInRange) _creatureInRange = null;
    }

    private void Update()
    {
        if (PlayerInputManager._scanIsPressed)
        {
            if (!PlayerMovementHandler._instance._isSwimming) return;

            transform.localScale = new Vector2(_dataScanRange, _dataScanRange);

            _spriteRenderer.color = _scanningColor;

            _collider.enabled = true;

            if (_creatureInRange == null) return;

            float currentTotal = 0;

            foreach (var val in _storedCreatures.Values) currentTotal += val;

            if (currentTotal >= _dataMaximum) return;

            if (_storedCreatures.ContainsKey(_creatureInRange))
            {
                _storedCreatures[_creatureInRange] += _dataAddAmount;
                TransformationUIDataHandler._instance.UpdateCreatureUI(_creatureInRange, _storedCreatures[_creatureInRange], false);
            }
            else
            {
                _storedCreatures[_creatureInRange] = _dataAddAmount;
                TransformationUIDataHandler._instance.UpdateCreatureUI(_creatureInRange, _storedCreatures[_creatureInRange], true);
            }

            _totalDataAmount += _dataAddAmount;

            TransformationUIDataHandler._instance.UpdateTotalDataText(_totalDataAmount, _dataMaximum);
        }
        else
        {
            _collider.enabled = false;

            _spriteRenderer.color = _normalColor;
        }
    }

    public float GetDataMaximum() => _dataMaximum;

    public float GetCurrentTotal()
    {
        float total = 0;
        foreach (var val in _storedCreatures.Values) total += val;
        return total;
    }

    public Dictionary<CreatureData, float> GetStoredCreatures()
    {
        return new Dictionary<CreatureData, float>(_storedCreatures);
    }

    public void UploadAllDataToComputer()
    {
        var computer = ComputerDataHolder._instance;

        foreach (var pair in _storedCreatures)
        {
            CreatureData creature = pair.Key;

            float amount = pair.Value;

            float existing = computer.GetCreatureDataAmount(creature);

            float newAmount = existing + amount;

            newAmount = Mathf.Clamp(newAmount, 0f, creature._creatureDataMaximum);

            computer.UploadCreatureData(creature, newAmount);
        }

        _storedCreatures.Clear();
        _totalDataAmount = 0f;

        TransformationUIDataHandler._instance.ClearUI();
    }
}