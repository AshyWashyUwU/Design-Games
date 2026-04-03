using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreatureHotbarButtonHandler : MonoBehaviour
{
    [SerializeField] private int _creatureSlotButton;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Color _filledColor;

    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private CreatureButtonType _creatureButtonType;

    private CreatureData _storedCreature;

    private Image _buttonImage;

    private bool _selected = false;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (_selected) SetSelected(!_selected);
    }

    public int GetCreatureSlotNum()
    {
        return _creatureSlotButton;
    }

    public void ChangeCreature(CreatureData _newCreatureData)
    {
        _buttonImage = GetComponent<Image>();

        _storedCreature = _newCreatureData;

        if (_storedCreature != null)
        {
            _buttonText.text = _storedCreature.name;
            _buttonImage.color = _filledColor;
        }
        else
        {
            _buttonText.text = "(No creature)";
            _buttonImage.color = _unselectedColor;
        }
    }

    public void SetSelected(bool selected)
    {
        _selected = selected;

        if (_selected)
        {
            _buttonText.text = "(Select a creature)";
            _buttonImage.color = _selectedColor;
        }
        else
        {
            if (_storedCreature != null)
            {
                _buttonText.text = _storedCreature.name;
                _buttonImage.color = _filledColor;
            }
            else
            {
                _buttonText.text = "(No creature)";
                _buttonImage.color = _unselectedColor;
            }
        }
    }

    public void ButtonPress()
    {
        ComputerUIEncyclopediaHandler._instance.UpdateStoredCreatureButton(this);
    }

    private enum CreatureButtonType
    {
        EncyclopediaButton,
        HotbarButton
    }
}
