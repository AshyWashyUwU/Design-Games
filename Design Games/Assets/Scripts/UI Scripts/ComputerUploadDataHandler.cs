using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerUploadDataHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _noDataText;
    [SerializeField] private GameObject _progressBarRoot;
    [SerializeField] private Image _progressFill;
    [SerializeField] private TextMeshProUGUI _timeText;

    [Header("Upload Settings")]
    [SerializeField] private float _uploadRate = 10f;
    [SerializeField] private float _lerpSpeed = 5f;

    private float _completionHoldTime;

    private float _holdTimer = 0f;
    private bool _isHolding = false;

    private float _uploadDuration;

    private float _currentTime;
    private bool _isUploading = false;

    private void OnEnable()
    {
        ResetUI();
    }

    private void Update()
    {
        if (!_isUploading) return;

        if (_isHolding)
        {
            _holdTimer += Time.deltaTime;

            _progressFill.fillAmount = 1f;
            _timeText.text = "Finalizing...";

            if (_holdTimer >= _completionHoldTime)
            {
                CompleteUpload();
            }

            return;
        }

        _currentTime += Time.deltaTime;

        float progress = Mathf.Clamp01(_currentTime / _uploadDuration);

        _progressFill.fillAmount = Mathf.Lerp(_progressFill.fillAmount, progress, _lerpSpeed * Time.deltaTime);

        float timeLeft = Mathf.Max(0f, _uploadDuration - _currentTime);
        _timeText.text = $"Uploading... {timeLeft:F2}s";

        if (_progressFill.fillAmount >= 0.999f)
        {
            _isHolding = true;
            _holdTimer = 0f;

            _progressFill.fillAmount = 1f;
        }
    }

    public void StartUpload()
    {
        if (_isUploading) return;

        var player = TransformationDeviceScanningHandler._instance;

        float totalData = player.GetCurrentTotal();

        if (totalData <= 0f)
        {
            ShowNoData();
            return;
        }

        _uploadDuration = totalData / _uploadRate;
        _uploadDuration = Mathf.Clamp(_uploadDuration, 0.5f, 30f);

        _isUploading = true;
        _currentTime = 0f;

        _noDataText.SetActive(false);
        _progressBarRoot.SetActive(true);

        _progressFill.fillAmount = 0f;
    }

    public void CancelUpload()
    {
        if (!_isUploading) return;

        _isUploading = false;

        ResetUI();
    }

    private void CompleteUpload()
    {
        _isUploading = false;

        TransformationDeviceScanningHandler._instance.UploadAllDataToComputer();

        ResetUI();
    }

    private void ResetUI()
    {
        _isUploading = false;
        _isHolding = false;
        _currentTime = 0f;
        _holdTimer = 0f;

        _completionHoldTime = Random.Range(0.2f, 1.5f);

        _progressBarRoot.SetActive(false);
        _noDataText.SetActive(true);

        if (_progressFill != null) _progressFill.fillAmount = 0f;

        if (_timeText != null) _timeText.text = "";
    }

    private void ShowNoData()
    {
        _progressBarRoot.SetActive(false);
        _noDataText.SetActive(true);
    }
}