using UnityEngine;
using System.Collections.Generic;

public class TransformationDeviceScanningHandler : MonoBehaviour
{
    private static TransformationDeviceScanningHandler Instance;
    public static TransformationDeviceScanningHandler _instance { get => Instance; }

    [Header("Toggable Variables")]
    [SerializeField] private Color _scanningColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private GameObject _ringPulsate;

    private float _dataMaximum = 100f;
    private float _dataScanRange = 7f;
    private float _totalDataAmount = 0f;
    private float _dataAddAmount = 0.01f;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private HashSet<CreatureData> _creaturesInRange = new HashSet<CreatureData>();

    private Dictionary<CreatureData, float> _storedCreatures = new Dictionary<CreatureData, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void UpdateDataCollectionAmount(float newCollectionAmount)
    {
        _dataAddAmount = newCollectionAmount;
    }

    public void UpdateDataMaximum(float newDataMaximum)
    {
        _storedCreatures.Clear();
        _totalDataAmount = 0f;

        TransformationUIDataHandler._instance.ClearUI();

        _dataMaximum = newDataMaximum;
    }

    public void UpdateDataScanRange(float newScanRange)
    {
        _dataScanRange = newScanRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICreature creatureComponent = collision.GetComponent<ICreature>();

        if (creatureComponent != null)
        {
            _creaturesInRange.Add(creatureComponent.GetCreatureData());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ICreature creatureComponent = collision.GetComponent<ICreature>();

        if (creatureComponent != null)
        {
            _creaturesInRange.Remove(creatureComponent.GetCreatureData());
        }
    }

    private void Update()
    {
        if (PlayerInputManager._scanIsPressed && PlayerMovementHandler._instance._isSwimming)
        {
            transform.localScale = new Vector2(_dataScanRange, _dataScanRange);

            _spriteRenderer.color = _scanningColor;
            _ringPulsate.SetActive(true);
            _collider.enabled = true;

            if (_creaturesInRange.Count == 0) return;

            float currentTotal = GetCurrentTotal();

            if (currentTotal >= _dataMaximum) return;

            foreach (var creature in _creaturesInRange)
            {
                if (currentTotal >= _dataMaximum) break;

                if (_storedCreatures.ContainsKey(creature))
                {
                    _storedCreatures[creature] += _dataAddAmount;
                    TransformationUIDataHandler._instance.UpdateCreatureUI(creature, _storedCreatures[creature], false);
                }
                else
                {
                    _storedCreatures[creature] = _dataAddAmount;
                    TransformationUIDataHandler._instance.UpdateCreatureUI(creature, _storedCreatures[creature], true);
                }

                _totalDataAmount += _dataAddAmount;
                currentTotal += _dataAddAmount;
            }

            TransformationUIDataHandler._instance.UpdateTotalDataText(_totalDataAmount, _dataMaximum);
        }
        else
        {
            _collider.enabled = false;

            _spriteRenderer.color = _normalColor;
            _ringPulsate.SetActive(false);
        }
    }

    public float GetDataMaximum() => _dataMaximum;

    public float GetCurrentTotal()
    {
        float total = 0f;

        foreach (var val in _storedCreatures.Values)
        {
            total += val;
        }

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