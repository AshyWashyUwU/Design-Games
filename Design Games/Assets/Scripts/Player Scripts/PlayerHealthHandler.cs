using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHandler : MonoBehaviour
{
    private static PlayerHealthHandler Instance;
    public static PlayerHealthHandler _instance => Instance;

    [Header("Health / Danger")]
    [SerializeField] private float _maxHealth = 100f;
    public float _currentHealth;

    [Header("UI")]
    [SerializeField] private Image _healthFill;
    [SerializeField] private CanvasGroup _healthCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _lerpSpeed = 5f;

    private float _displayedFill;

    [Header("Danger Decay")]
    [SerializeField] private float _dangerResetDelay = 6.5f;

    private float _lastDamageTime;
    private float _decayVelocity;

    [Header("Knockback")]
    [SerializeField] private float _knockbackForce = 7.5f;
    private Rigidbody2D _rb;

    public bool _alreadyDead = false;

    private Coroutine _fadeRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = 0f;
        _displayedFill = 0f;

        if (_healthCanvasGroup != null)
        {
            _healthCanvasGroup.alpha = 0f;
            _healthCanvasGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        HandleDangerDecay();
        UpdateHealthUI();
    }


    public void TakeDamage(float amount, Vector2 hitDirection)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        _lastDamageTime = Time.time;

        ShowHealthUI();

        var movement = GetComponent<PlayerMovementHandler>();
        if (movement != null)
        {
            movement.ApplyKnockback(hitDirection * _knockbackForce, 0.2f);
        }

        if (_currentHealth >= _maxHealth)
        {
            Die();
        }
    }

    private void HandleDangerDecay()
    {
        if (_currentHealth <= 0f) return;

        if (Time.time - _lastDamageTime < _dangerResetDelay)
            return;

        _currentHealth = Mathf.SmoothDamp(_currentHealth, 0f, ref _decayVelocity, 0.5f);

        if (_currentHealth <= 0.01f)
        {
            _currentHealth = 0f;
            HideHealthUI();
        }
    }

    private void UpdateHealthUI()
    {
        if (_healthFill == null) return;

        float targetFill = _currentHealth / _maxHealth;

        float speed = (_displayedFill < targetFill) ? _lerpSpeed * 2f : _lerpSpeed;

        _displayedFill = Mathf.Lerp(_displayedFill, targetFill, speed * Time.deltaTime);
        _healthFill.fillAmount = _displayedFill;
    }

    private void ShowHealthUI()
    {
        if (_healthCanvasGroup == null) return;

        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeHealthUI(1f));
    }

    private void HideHealthUI()
    {
        if (_healthCanvasGroup == null) return;

        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeHealthUI(0f));
    }

    private IEnumerator FadeHealthUI(float targetAlpha)
    {
        float startAlpha = _healthCanvasGroup.alpha;
        float time = 0f;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _healthCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            yield return null;
        }

        _healthCanvasGroup.alpha = targetAlpha;
        _healthCanvasGroup.blocksRaycasts = targetAlpha > 0f;
    }

    private void Die()
    {
        if (_alreadyDead) return;

        _alreadyDead = true;

        TogglePlayerMovementHandler._instance.Interact(gameObject);

        StartCoroutine(ClearCreatures());
    }

    private IEnumerator ClearCreatures()
    {
        yield return new WaitForSeconds(0.5f);
        TransformationDeviceScanningHandler._instance.ClearStoredCreatures();        
    }

    public void FullyHeal()
    {
        _currentHealth = 0f;

        _lastDamageTime = Time.time;

        _alreadyDead = false;

        HideHealthUI();
    }
}