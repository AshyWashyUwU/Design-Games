using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ComputerEncyclopediaEntryHandler : MonoBehaviour
{
    private static ComputerEncyclopediaEntryHandler Instance;

    public static ComputerEncyclopediaEntryHandler _instance { get => Instance; }

    [SerializeField] private TextMeshProUGUI _creatureNameText;
    [SerializeField] private TextMeshProUGUI _researchRequiredText;
    [SerializeField] private TextMeshProUGUI _creatureTypeText;
    [SerializeField] private TextMeshProUGUI _creaturePreyText;
    [SerializeField] private TextMeshProUGUI _creaturePredatorText;
    [SerializeField] private TextMeshProUGUI _creatureHabitatText;
    [SerializeField] private TextMeshProUGUI _creatureDescriptionText;

    [SerializeField] private Image _creatureImage;

    [SerializeField] private GameObject _creatureNoImageText;

    [SerializeField] private List<Image> _colorBoxes = new List<Image>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowEncyclopediaEntry(CreatureData _creature, float _dataPercentage, Color _unlockedColor)
    {
        for (int i = 0; i < _colorBoxes.Count; i++)
        {
            _colorBoxes[i].color = _unlockedColor;
        }

        if (_dataPercentage > 0)
        {
            _creatureNameText.text = _creature.name;
            _researchRequiredText.text = "Research Required: " + _creature._creatureDataMaximum + " GB";

            _creatureImage.gameObject.SetActive(true);

            _creatureImage.sprite = _creature._creatureSprite;

            _creatureNoImageText.SetActive(false);
        }
        else
        {
            _creatureNameText.text = "???";
            _researchRequiredText.text = "Research Required: ???";

            _creatureImage.gameObject.SetActive(false);

            _creatureNoImageText.SetActive(true);
        }

        if (_dataPercentage > 25)
        {
            _creatureTypeText.text = "Type: " + GetCreatureType(_creature._creatureType.ToString());

            string _creaturePreyList = "";

            if (_creature._creaturePrey.Count != 0)
            {
                for (int i = 0; i < _creature._creaturePrey.Count; i++)
                {
                    if (i != 0 && i != _creature._creaturePrey.Count)
                    {
                        _creaturePreyList = _creaturePreyList + ", " + _creature._creaturePrey[i].name;
                    }
                    else
                    {
                        _creaturePreyList = _creaturePreyList + " " + _creature._creaturePrey[i].name;                    
                    }
                }
            }
            else
            {
                _creaturePreyList = " N/A";
            }

            _creaturePreyText.text = "Prey:" + _creaturePreyList;

            string _creaturePredatorList = "";

            for (int i = 0; i < CreatureDataHolder._instance.GetCreatureList().Count; i++)
            {
                for (int j = 0; j < CreatureDataHolder._instance.GetCreatureList()[i]._creaturePrey.Count; j++)
                {
                    if (CreatureDataHolder._instance.GetCreatureList()[i]._creaturePrey[j].name == _creature.name)
                    {
                        if (i != 0 && i != CreatureDataHolder._instance.GetCreatureList()[j]._creaturePrey.Count)
                        {
                            _creaturePredatorList = _creaturePredatorList + ", " + CreatureDataHolder._instance.GetCreatureList()[i].name;
                        }
                        else
                        {
                            _creaturePredatorList = _creaturePredatorList + " " + CreatureDataHolder._instance.GetCreatureList()[i].name;
                        }
                    }
                }
            }

            _creaturePredatorText.text = "Predator(s):" + _creaturePredatorList;

            string _creatorHabitatList = "";

            for (int i = 0; i < _creature._creatureLayers.Count; i++)
            {
                if (i != 0 && i != _creature._creatureLayers.Count)
                {
                    _creatorHabitatList = _creatorHabitatList + ", " + "Layer " + _creature._creatureLayers[i];
                }
                else
                {
                    _creatorHabitatList = _creatorHabitatList + " " + "Layer " + _creature._creatureLayers[i];
                }
            }

            _creatureHabitatText.text = "Habitat:" + _creatorHabitatList;

            _creatureDescriptionText.text = "Description: " + _creature._creatureDescription;
        }
        else
        {
            _creatureTypeText.text = "Type: ???";
            _creaturePreyText.text = "Prey: ???";
            _creaturePredatorText.text = "Predator(s): ???";
            _creatureHabitatText.text = "Habitat: ???";
            _creatureDescriptionText.text = "Description: ???";
        }
    }

    private string GetCreatureType(string _creatureTypeToConvert)
    {
        if (_creatureTypeToConvert == "_producer")
        {
            return "Producer";
        }
        if (_creatureTypeToConvert == "_primaryConsumer")
        {
            return "Primary Consumer";
        }
        if (_creatureTypeToConvert == "_secondaryConsumer")
        {
            return "Secondary Consumer";
        }
        if (_creatureTypeToConvert == "_apexPredator")
        {
            return "Apex Predator";
        }
        else
        {
            return "Unclassified";
        }
    }
}
