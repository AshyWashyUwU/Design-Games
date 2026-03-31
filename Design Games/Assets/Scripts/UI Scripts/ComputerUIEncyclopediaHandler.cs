using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ComputerUIEncyclopediaHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonParent;

    private List<GameObject> _spawnedButtons = new List<GameObject>();

    private void OnEnable()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (var btn in _spawnedButtons)
        {
            Destroy(btn);
        }

        _spawnedButtons.Clear();

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

            ui.SetCreatureButton(percent, _creatureNum, creature.name);
        }
    }
}