using UnityEngine;
using TMPro;

public class TransformationUIOpenCloseHandler : MonoBehaviour
{
    [SerializeField] private GameObject _openableCreatureUI;
    [SerializeField] private Transform _openCloseButton;

    [SerializeField] private Transform _openCloseButtonPosOpen;
    [SerializeField] private Transform _openCloseButtonPosClosed;

    [SerializeField] private GameObject _timer;

    private TextMeshProUGUI _openCloseButtonText;

    private void Awake()
    {
        _openCloseButtonText = _openCloseButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void ToggleUIOpenClose()
    {
        _openableCreatureUI.SetActive(!_openableCreatureUI.activeSelf);

        _timer.SetActive(!_timer.activeSelf);

        if (_openableCreatureUI.activeSelf)
        {
            _openCloseButton.transform.position = _openCloseButtonPosOpen.transform.position;
            _openCloseButtonText.text = "<";
        }
        else
        {
            _openCloseButton.transform.position = _openCloseButtonPosClosed.transform.position;
            _openCloseButtonText.text = ">";
        }
    }
}
