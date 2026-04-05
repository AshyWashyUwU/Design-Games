using UnityEngine;

public class TransformFlashHandler : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _spawnTrigger = "SpawnRing";

    [Header("Color Settings")]
    [SerializeField] private Renderer[] _partsToColor;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _timeoutColor = Color.red;

    [Header("Timing")]
    [SerializeField] private float _autoDestroyTime = 1.5f;

    private void Awake()
    {
        if (_animator != null) _animator.SetTrigger(_spawnTrigger);

        float animLength = _autoDestroyTime;
        if (_animator != null && _animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                animLength = clips[0].length;
            }
        }

        Destroy(gameObject, animLength);
    }

    public void SetTimeoutColor(bool isTimeout)
    {
        Color targetColor = isTimeout ? _timeoutColor : _normalColor;

        foreach (Renderer r in _partsToColor)
        {
            if (r != null)
            {
                foreach (Material mat in r.materials)
                {
                    mat.color = targetColor;
                }
            }
        }
    }
}