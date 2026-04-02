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

    private void RefreshUI()
    {
        ComputerEncyclopediaEntryHandler._instance.ForceOutComputerMode();

        foreach (var btn in _spawnedButtons)
        {
            Destroy(btn);
        }

        _spawnedButtons.Clear();
        _notAbleToTransformButtons.Clear();

        var creatures = CreatureDataHolder._instance.GetCreatureList();

        int _creatureNum = 0;

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

    public void ChangeNotTransformButtons()
    {
        foreach(GameObject button in _notAbleToTransformButtons)
        {
            button.SetActive(!button.activeSelf);
        }
    }
}