using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TransformationDeviceHandler : MonoBehaviour
{
    private static TransformationDeviceHandler Instance;
    public static TransformationDeviceHandler _instance => Instance;

    public CreatureData _currentTransformedCreature;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _smallTimerText;
    [SerializeField] private TextMeshProUGUI _largeTimerText;
    [SerializeField] private Image[] _radialFillImages;
    [SerializeField] private Image _vignetteImage;

    [Header("Timing")]
    [SerializeField] private int _transformTime = 30;
    [SerializeField] private int _transformCooldownTime = 30;

    [Header("Cooldown Tuning")]
    [SerializeField] private float _minCooldownPercent = 0.3f;
    [SerializeField] private float _cooldownCurvePower = 1.5f;

    [Header("Warning Settings")]
    [SerializeField] private float _warningThreshold = 5f;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _warningColor = Color.red;

    [Header("Vignette Settings")]
    [SerializeField] private float _vignetteMaxAlpha = 0.5f;
    [SerializeField] private float _vignetteFadeSpeed = 2f;

    [Header("Text")]
    [SerializeField] private TextMeshPro _playerText;
    [SerializeField] private TextMeshProUGUI _transformedCreatureUIText;

    [Header("Transform Flash Prefab")]
    [SerializeField] private GameObject _transformFlashPrefab;

    private bool _isTransforming = false;
    private bool _isOnCooldown = false;
    private bool _isBusy = false;

    private float _currentTransformTimeLeft;
    private float _vignettePulseTime = 0f;

    private Coroutine _activeRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Transform(CreatureData newCreature)
    {
        if (_isBusy) return;
        if (!PlayerMovementHandler._instance._isSwimming) return;

        if (newCreature == _currentTransformedCreature)
        {
            if (!_isOnCooldown && _isTransforming)
                Detransform(false);
            return;
        }

        if (_isTransforming || _isOnCooldown) return;

        StartNewRoutine(TransformRoutine(newCreature));
    }

    private IEnumerator TransformRoutine(CreatureData newCreature)
    {
        _isBusy = true;
        _isTransforming = true;

        PlayTransformFlash(false);

        yield return new WaitForSeconds(0.75f);

        _currentTransformedCreature = newCreature;
        _playerText.text = newCreature.name;
        _transformedCreatureUIText.text = newCreature.name;

        float timeLeft = _transformTime;
        _currentTransformTimeLeft = timeLeft;

        _isBusy = false;

        while (timeLeft > 0 && _currentTransformedCreature != null)
        {
            timeLeft -= Time.deltaTime;
            _currentTransformTimeLeft = timeLeft;

            UpdateTimers(timeLeft, _transformTime, false);

            yield return null;
        }

        Detransform(true);
    }

    public void Detransform(bool isTimeout = false)
    {
        if (!_isTransforming || _currentTransformedCreature == null || _isBusy) return;

        _isTransforming = false;
        _isOnCooldown = true;

        PlayTransformFlash(isTimeout);

        float cooldown = CalculateCooldown();

        StartNewRoutine(CooldownRoutine(cooldown));

        StartCoroutine(FadeVignetteCoroutine(0f));
    }

    private IEnumerator CooldownRoutine(float cooldownDuration)
    {
        _isBusy = true;

        yield return new WaitForSeconds(0.75f);

        _playerText.text = "Player";
        _transformedCreatureUIText.text = "(No creature selected)";
        _currentTransformedCreature = null;

        float cooldownLeft = cooldownDuration;
        _isBusy = false;

        while (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
            UpdateTimers(cooldownLeft, cooldownDuration, true);
            yield return null;
        }

        _isOnCooldown = false;

        _smallTimerText.text = "";
        _largeTimerText.text = "";

        foreach (Image _radialImage in _radialFillImages)
        {
            _radialImage.fillAmount = 0f;
        }
        
        while (_vignetteImage.color.a > 0f)
        {
            FadeVignette(0f, false);
            yield return null;
        }
    }

    private float CalculateCooldown()
    {
        float usagePercent = 1f - (_currentTransformTimeLeft / _transformTime);
        usagePercent = Mathf.Clamp01(usagePercent);
        usagePercent = Mathf.Pow(usagePercent, _cooldownCurvePower);

        float minCooldown = _transformCooldownTime * _minCooldownPercent;

        return Mathf.Lerp(minCooldown, _transformCooldownTime, usagePercent);
    }

    private void UpdateTimers(float time, float maxTime, bool isCooldown)
    {
        time = Mathf.Max(0f, time);

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        _smallTimerText.text = $"{minutes}:{seconds:00}";
        _largeTimerText.text = $"{(isCooldown ? "Transform Cooldown: " : "Transform Time: ")}{minutes}:{seconds:00}";

        foreach (Image _radialImage in _radialFillImages)
        {
            _radialImage.fillAmount = time / maxTime;
        }

        if (!isCooldown && time <= _warningThreshold)
        {
            if (time <= 0.05f)
            {
                _smallTimerText.color = _warningColor;
                _largeTimerText.color = _warningColor;

                foreach (Image _radialImage in _radialFillImages)
                {
                   _radialImage.color = _warningColor;
                }

                _vignettePulseTime = 0f;
                return;
            }

            float dangerPercent = 1f - (time / _warningThreshold);
            float speed = Mathf.Lerp(1f, 4f, dangerPercent);

            _vignettePulseTime += Time.deltaTime * speed;
            float pulse = Mathf.PingPong(_vignettePulseTime, 1f);

            Color flashColor = Color.Lerp(_normalColor, _warningColor, pulse);

            _smallTimerText.color = flashColor;
            _largeTimerText.color = flashColor;

            foreach (Image _radialImage in _radialFillImages)
            {
               _radialImage.color = flashColor;
            }

            float vignettePulse = Mathf.SmoothStep(0f, 1f, pulse);
            float targetAlpha = vignettePulse * _vignetteMaxAlpha;

            FadeVignette(targetAlpha, false);
        }
        else
        {
            _smallTimerText.color = _normalColor;
            _largeTimerText.color = _normalColor;

            foreach (Image _radialImage in _radialFillImages)
            {
               _radialImage.color = _normalColor;
            }

            FadeVignette(0f, false);
            _vignettePulseTime = 0f;
        }
    }

    private void FadeVignette(float targetAlpha, bool instant)
    {
        if (_vignetteImage == null) return;

        Color c = _vignetteImage.color;

        if (instant)
        {
            c.a = targetAlpha;
        }
        else
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * _vignetteFadeSpeed);
        }

        _vignetteImage.color = c;
    }

    private IEnumerator FadeVignetteCoroutine(float targetAlpha)
    {
        if (_vignetteImage == null) yield break;

        while (!Mathf.Approximately(_vignetteImage.color.a, targetAlpha))
        {
            FadeVignette(targetAlpha, false);
            yield return null;
        }
    }

    private void StartNewRoutine(IEnumerator routine)
    {
        if (_activeRoutine != null)
            StopCoroutine(_activeRoutine);

        _activeRoutine = StartCoroutine(routine);
    }

    private void PlayTransformFlash(bool isTimeout)
    {
        if (_transformFlashPrefab == null) return;

        GameObject flash = Instantiate(_transformFlashPrefab, transform.position, Quaternion.identity, transform);
        TransformFlashHandler flashScript = flash.GetComponent<TransformFlashHandler>();

        if (flashScript != null) flashScript.SetTimeoutColor(isTimeout);
    }
}