using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class TogglePlayerMovementHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerTeleportPoint;

    [SerializeField] private CanvasGroup _loadingScreen;

    [SerializeField] private string _customText;
    [SerializeField] private Vector3 _customOffset;

    [SerializeField] private CinemachineCamera _camera;

    [SerializeField] private float _newLensSize;

    private bool _alreadyTeleported = false;

    public void Interact(GameObject _interactor)
    {
        if (_alreadyTeleported) return;

        _alreadyTeleported = true;

        PlayerMovementHandler._instance.ToggleImmobilization();

        StartCoroutine(FadeLoadingScreen(1, 0.35f));
    }

    private IEnumerator FadeLoadingScreen(float _targetAlpha, float _duration)
    {
        float startAlpha = _loadingScreen.alpha;
        float _time = 0f;

        while (_time < _duration)
        {
            _time += Time.unscaledDeltaTime;
            _loadingScreen.alpha = Mathf.Lerp(startAlpha, _targetAlpha, _time / _duration);
            yield return null;
        }

        _loadingScreen.alpha = _targetAlpha;

        _loadingScreen.blocksRaycasts = !_loadingScreen.blocksRaycasts;

        if (_targetAlpha != 0)
        {

            PlayerMovementHandler._instance.GetPlayerTransform().position = _playerTeleportPoint.transform.position;

            PlayerMovementHandler._instance.TogglePlayerMovementType();

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(FadeLoadingScreen(0, _duration));

            var lens = _camera.Lens;
            lens.OrthographicSize = _newLensSize;
            _camera.Lens = lens;
        }
        else
        {
            _alreadyTeleported = false;
            PlayerMovementHandler._instance.ToggleImmobilization();
        }
    }

    public string GetInteractText()
    {
        return _customText;
    }

    public Vector3 GetInteractOffset()
    {
        return _customOffset;
    }
}