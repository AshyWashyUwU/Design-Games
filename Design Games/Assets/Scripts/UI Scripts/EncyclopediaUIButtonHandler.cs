using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EncyclopediaUIButtonHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText, _percentText, _numberText;

    [SerializeField] private Color _isTransformationColor, _fullyUnlockedColor, _unlockedColor, _lockedColor;

    private Image _buttonImage;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
    }

    public void SetCreatureButton(float _creaturePercent, int _creatureNum, string _creatureName)
    {
        if (_creaturePercent > 0)
        {
            _nameText.text = $"{_creatureName}";

            _percentText.text = $"{_creaturePercent:0}%";

            _numberText.text = $"{_creatureNum:D3}";

            if (_creaturePercent > 70)
            {
                _buttonImage.color = _fullyUnlockedColor;
            }
            else
            {
                _buttonImage.color = _unlockedColor;
            }
        }
        else
        {
            _buttonImage.color = _lockedColor;

            _nameText.text = "???";

            _percentText.text = "";

            _numberText.text = "";
        }
    }
}