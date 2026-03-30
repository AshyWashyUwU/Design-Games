using UnityEngine;
using TMPro;
using System.Collections;

public class InteractUIHandler : MonoBehaviour
{
    private static InteractUIHandler Instance;
    public static InteractUIHandler _instance { get => Instance; }

    [SerializeField] private TextMeshProUGUI _interactedText;

    private Vector3 _offset = new Vector3(0, 3, 0);

    public GameObject _interactedObject { get; private set; }

    private string _currentText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateInteractedObject(GameObject _object, string _customName, Vector3 _customOffset)
    {
        if (gameObject == null) return;

        if (_interactedObject == null)
        {
            _interactedObject = _object;
        }
        else if (_interactedObject == _object)
        {
            _interactedObject = null;
        }

        if (_interactedObject == null)
        {
            _interactedText.text = "";
        }
        else
        {
            _currentText = !string.IsNullOrEmpty(_customName) ? _customName : _interactedObject.name;

            _interactedText.transform.position = _interactedObject.transform.position + _customOffset;

            _interactedText.text = "E - " + _currentText;
        }
    }
}