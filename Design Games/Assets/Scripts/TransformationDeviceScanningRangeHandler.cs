using UnityEngine;
using System.Collections.Generic;

public class TransformationDeviceScanningHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Color _scanningColor;
    [SerializeField] private Color _normalColor;

    private float _dataMaximum = 100f;
    private float _totalDataAmount = 0f;
    private float _dataAddAmount = 0.01f;

    private SpriteRenderer _spriteRenderer;

    private Dictionary<CreatureData, float> _storedCreatures = new Dictionary<CreatureData, float>();
    private CreatureData _creatureInRange;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        ICreature _creatureComponent = _collision.GetComponent<ICreature>();
        if (_creatureComponent != null) _creatureInRange = _creatureComponent.GetCreatureData();
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        ICreature creatureComponent = _collision.GetComponent<ICreature>();
        if (creatureComponent != null && creatureComponent.GetCreatureData() == _creatureInRange) _creatureInRange = null;
    }

    private void Update()
    {
        if (_creatureInRange != null && PlayerInputManager.scanIsPressed)
        {
            _spriteRenderer.color = _scanningColor;

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
}