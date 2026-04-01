using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EncyclopediaUIButtonHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText, _percentText, _numberText;

    [SerializeField] private Color _isTransformationColor, _fullyUnlockedColor, _unlockedColor, _lockedColor;

    [SerializeField] private Image _innerButtonImage;

    [SerializeField] private Image _outerButtonImage;

    private CreatureData _storedCreature;

    private float _storedPercent;

    private Color _storedColor;

    public void SetCreatureButton(CreatureData _creature, float _creaturePercent, int _creatureNum, string _creatureName)
    {
        _storedCreature = _creature;

        _storedPercent = _creaturePercent;

        if (_creaturePercent > 0)
        {
            _nameText.text = $"{_creatureName}";

            _percentText.text = $"{_creaturePercent:0}%";

            _outerButtonImage.fillAmount = _creaturePercent / 100;

            _numberText.text = $"{_creatureNum:D3}";

            if (_creaturePercent > 70)
            {
                _innerButtonImage.color = _fullyUnlockedColor;
            }
            else
            {
                _innerButtonImage.color = _unlockedColor;
            }
        }
        else
        {
            _innerButtonImage.color = _lockedColor;

            _nameText.text = "???";

            _percentText.text = "";

            _outerButtonImage.fillAmount = 0;

            _numberText.text = "";
        }

        _storedColor = _innerButtonImage.color;
    }

    public void OpenCreatureEntry()
    {
        ComputerEncyclopediaEntryHandler._instance.ShowEncyclopediaEntry(_storedCreature, _storedPercent, _storedColor);
    }
}