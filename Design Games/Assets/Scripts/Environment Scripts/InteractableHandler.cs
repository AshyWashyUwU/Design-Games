using UnityEngine;

public class InteractableHandler : MonoBehaviour
{
    private IInteractable _interactable;

    private Animator _animator;

    private void Awake()
    {
        _interactable = GetComponent<IInteractable>();

        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            PlayerInteractionHandler _player = _other.GetComponent<PlayerInteractionHandler>();

            _player.SetInteractable(_interactable, gameObject);

            if (_animator != null)
            {
                _animator.SetTrigger("Interact");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            PlayerInteractionHandler _player = _other.GetComponent<PlayerInteractionHandler>();

            _player.ClearInteractable(gameObject);

            if (_animator != null)
            {
                _animator.SetTrigger("Unteract");
            }
        }
    }
}