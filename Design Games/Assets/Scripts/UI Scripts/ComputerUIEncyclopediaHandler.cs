using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ComputerUIEncyclopediaHandler : MonoBehaviour
{
    private static ComputerUIEncyclopediaHandler Instance;

    public static ComputerUIEncyclopediaHandler _instance { get => Instance; }

    [Header("References")]
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonParent;

    public List<GameObject> _spawnedButtons = new List<GameObject>();
    public List<GameObject> _notAbleToTransformButtons = new List<GameObject>();

    public CreatureHotbarButtonHandler _storedCreatureButtonHandler;

    [SerializeField] private CreatureHotbarHandler _hotbarHandler;

    [SerializeField] private TextMeshProUGUI _noTransformationsText;

    private bool _isActive = true;

    int _creatureNum = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        _storedCreatureButtonHandler = null;
    }

    public void UpdateStoredCreatureButtonWithNewCreature(CreatureData newCreature)
    {
        if (_storedCreatureButtonHandler == null) return;

        int slot = _storedCreatureButtonHandler.GetCreatureSlotNum();

        CreatureSlotHolder._instance.ChangeCreatureSlot(newCreature, slot);

        _storedCreatureButtonHandler.SetSelected(false);
        _storedCreatureButtonHandler = null;

        _hotbarHandler.UpdateList();
        RefreshUI();
    }

    public void UpdateStoredCreatureButton(CreatureHotbarButtonHandler newButton)
    {
        if (_storedCreatureButtonHandler == newButton)
        {
            _isActive = true;
            ChangeNotTransformButtons();
            ComputerEncyclopediaEntryHandler._instance.ChangeComputerMode(false);
            newButton.SetSelected(false);
            _storedCreatureButtonHandler = null;
            return;
        }

        if (newButton != _storedCreatureButtonHandler && _storedCreatureButtonHandler != null)
        {
            _storedCreatureButtonHandler.SetSelected(false);
        }

        _isActive = false;
        ComputerEncyclopediaEntryHandler._instance.ChangeComputerMode(true);

        _storedCreatureButtonHandler = newButton;
        _storedCreatureButtonHandler.SetSelected(true);

        ChangeNotTransformButtons();
    }

    private void RefreshUI()
    {
        _isActive = true;
        _noTransformationsText.gameObject.SetActive(false);

        ComputerEncyclopediaEntryHandler._instance.ForceOutComputerMode();

        foreach (var btn in _spawnedButtons)
        {
            Destroy(btn);
        }

        _spawnedButtons.Clear();
        _notAbleToTransformButtons.Clear();

        var creatures = CreatureDataHolder._instance.GetCreatureList();

        _creatureNum = 0;

        foreach (var creature in creatures)
        {
            _creatureNum++;

            GameObject button = Instantiate(_buttonPrefab, _buttonParent);
            _spawnedButtons.Add(button);

            float stored = ComputerDataHolder._instance.GetCreatureDataAmount(creature);

            float percent = 0f;
            if (creature._creatureDataMaximum > 0)
            {
                percent = (stored / creature._creatureDataMaximum) * 100f;
            }

            var ui = button.GetComponent<EncyclopediaUIButtonHandler>();

            ui.SetCreatureButton(creature, percent, _creatureNum, creature.name);

            if (percent < 70)
            {
                _notAbleToTransformButtons.Add(button);
            }
        }
    }

    private void ChangeNotTransformButtons()
    {
        foreach(GameObject button in _notAbleToTransformButtons)
        {
            button.SetActive(_isActive);
        }

        if (_creatureNum == _notAbleToTransformButtons.Count && !_isActive)
        {
            _noTransformationsText.gameObject.SetActive(true);
        }
        else
        {
           _noTransformationsText.gameObject.SetActive(false);
        }
    }
}