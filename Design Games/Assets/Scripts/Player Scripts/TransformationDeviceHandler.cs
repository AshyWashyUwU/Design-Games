using UnityEngine;
using TMPro;
using System.Collections;

public class TransformationDeviceHandler : MonoBehaviour
{
    private static TransformationDeviceHandler Instance;
    public static TransformationDeviceHandler _instance { get => Instance; }

    public CreatureData _currentTransformedCreature;

    [SerializeField] private TextMeshProUGUI _timerText;

    [SerializeField] private int _transformTime = 2;
    [SerializeField] private int _transformCooldownTime = 2;

    [SerializeField] private TextMeshPro _playerText;

    [SerializeField] private Animator _ringAnimator;

    private bool _isTransforming = false;
    private bool _isOnCooldown = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Transform(CreatureData newCreature)
    {
        if (_isTransforming || _isOnCooldown)
        {
            return;
        }

        StartCoroutine(TransformRoutine(newCreature));
    }

    private IEnumerator TransformRoutine(CreatureData newCreature)
    {
        _isTransforming = true;

        _ringAnimator.SetTrigger("SpawnRing");

        yield return new WaitForSeconds(0.5f);

        _currentTransformedCreature = newCreature;

        _playerText.text = newCreature.name;

        float timeLeft = _transformTime;

        while (timeLeft > 0 && _currentTransformedCreature != null)
        {
            UpdateTimerUI(timeLeft);
            yield return null;
            timeLeft -= Time.deltaTime;
        }

        Detransform();
    }

    private IEnumerator CooldownRoutine()
    {
        _ringAnimator.SetTrigger("SpawnRing");

        yield return new WaitForSeconds(0.5f);

        _playerText.text = "Player";

        _currentTransformedCreature = null;

        _isOnCooldown = true;

        float cooldownLeft = _transformCooldownTime;

        while (cooldownLeft > 0)
        {
            UpdateTimerUI(cooldownLeft, true);
            yield return null;
            cooldownLeft -= Time.deltaTime;
        }

        _isOnCooldown = false;
        _timerText.text = "";
    }

    private void UpdateTimerUI(float time, bool isCooldown = false)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        string prefix = isCooldown ? "Timer Cooldown: " : "Transform Time: ";
        _timerText.text = $"{prefix}{minutes}:{seconds:00}";
    }

    public void Detransform()
    {
        if (_currentTransformedCreature == null || !_isTransforming) return;

        _isTransforming = false;

        StartCoroutine(CooldownRoutine());
    }
}