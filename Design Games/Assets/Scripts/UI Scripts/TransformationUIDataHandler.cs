using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TransformationUIDataHandler : MonoBehaviour
{
    private static TransformationUIDataHandler Instance;
    public static TransformationUIDataHandler _instance { get => Instance; }

    [Header("Pie Chart References")]
    [SerializeField] private GameObject _creaturePieChartPrefab;
    [SerializeField] private Transform _pieChartParent;
    [SerializeField] private TextMeshProUGUI _totalDataText;

    private float _lerpSpeed = 5f;

    private List<CreatureData> _creatureOrder = new List<CreatureData>();
    private Dictionary<CreatureData, SliceInfo> _creatureSlices = new Dictionary<CreatureData, SliceInfo>();
    private Dictionary<CreatureData, float> _creatureData = new Dictionary<CreatureData, float>();

    private class SliceInfo
    {
        public Image sliceImage;
        public Transform sliceTransform;
        public float currentFill = 0f;
        public float targetFill = 0f;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTotalDataText(float _totalDataAmount, float _dataMaximum)
    {
        if (_dataMaximum != 0)
        {
            float _percentage = (_totalDataAmount / _dataMaximum) * 100f;

            _totalDataText.text = "Data: " + _percentage.ToString("F0") + "%";
        }
        else
        {
            _totalDataText.text = "0%";
        }
    }

    public void UpdateCreatureUI(CreatureData creature, float creatureAmount, bool isNewCreature)
    {
        _creatureData[creature] = creatureAmount;

        if (isNewCreature && !_creatureSlices.ContainsKey(creature))
        {
            _creatureOrder.Add(creature);

            GameObject parentObj = new GameObject(creature.name + "_SliceParent");
            parentObj.transform.SetParent(_pieChartParent, false);
            parentObj.transform.localScale = Vector3.one;
            parentObj.transform.localRotation = Quaternion.identity;

            GameObject sliceObj = Instantiate(_creaturePieChartPrefab, parentObj.transform);
            sliceObj.transform.localScale = Vector3.one;
            sliceObj.transform.localRotation = Quaternion.identity;

            Image sliceImage = sliceObj.GetComponent<Image>();
            sliceImage.color = GetColorFromCreature(creature);
            sliceImage.fillAmount = 0f;

            _creatureSlices[creature] = new SliceInfo
            {
                sliceImage = sliceImage,
                sliceTransform = parentObj.transform,
                currentFill = 0f,
                targetFill = 0f
            };
        }
    }

    private void Update()
    {
        AnimatePie();
    }

    private void AnimatePie()
    {
        if (_creatureData.Count == 0) return;

        TransformationDeviceScanningHandler dataHandler = FindFirstObjectByType<TransformationDeviceScanningHandler>();
        if (dataHandler == null) return;

        float totalData = dataHandler.GetCurrentTotal();
        float dataMaximum = dataHandler.GetDataMaximum();

        if (totalData == 0 || dataMaximum == 0) return;

        float overallPercent = Mathf.Clamp01((float)totalData / dataMaximum);

        foreach (var creature in _creatureOrder)
        {
            SliceInfo info = _creatureSlices[creature];
            float amount = _creatureData.ContainsKey(creature) ? _creatureData[creature] : 0;
            float sliceFraction = (float)amount / totalData;
            info.targetFill = sliceFraction * overallPercent;
        }

        float cumulativeAngle = 0f;
        foreach (var creature in _creatureOrder)
        {
            SliceInfo info = _creatureSlices[creature];

            info.currentFill = Mathf.Lerp(info.currentFill, info.targetFill, _lerpSpeed * Time.deltaTime);
            if (Mathf.Abs(info.currentFill - info.targetFill) < 0.001f) info.currentFill = info.targetFill;

            info.sliceImage.fillAmount = info.currentFill;

            info.sliceTransform.localRotation = Quaternion.Euler(0f, 0f, -cumulativeAngle);

            cumulativeAngle += info.currentFill * 360f;
        }
    }

    private Color GetColorFromCreature(CreatureData creature)
    {
        int hash = Mathf.Abs(creature.name.GetHashCode());
        float hue = (hash % 360f) / 360f;
        float saturation = 0.69f;
        float value = 0.95f;
        return Color.HSVToRGB(hue, saturation, value);
    }
}