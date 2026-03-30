using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PiechartCreatureNameHandler : MonoBehaviour
{
    private CreatureData _storedCreatureData;
    private Image _piechartImage;
    private TextMeshProUGUI _creatureNameText;

    private void Awake()
    {
        _piechartImage = GetComponent<Image>();
    }

    public void SetCreatureData(CreatureData _newCreatureData)
    {
        _storedCreatureData = _newCreatureData;
    }

    public void SetCreatureNameText(TextMeshProUGUI _newText)
    {
        _creatureNameText = _newText;
    }

    private void Update()
    {
        if (_creatureNameText == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Canvas canvas = _piechartImage.canvas;
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_piechartImage.rectTransform, mousePos, cam, out localPoint)) return;

        Rect rect = _piechartImage.rectTransform.rect;
        float radius = rect.width * 0.5f;

        if (localPoint.magnitude > radius)
        {
            _creatureNameText.gameObject.SetActive(false);
            return;
        }

        Vector2 dir = localPoint.normalized;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        float fillAngle = _piechartImage.fillAmount * 360f;

        _creatureNameText.gameObject.SetActive(angle <= fillAngle);

        if (_storedCreatureData != null) _creatureNameText.text = _storedCreatureData.name;

        _creatureNameText.color = _piechartImage.color;
    }
}