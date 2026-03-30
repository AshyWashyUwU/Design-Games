using UnityEngine;

public class InteractableHandler : MonoBehaviour
{
    private IInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<IInteractable>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            PlayerInteractionHandler _player = _other.GetComponent<PlayerInteractionHandler>();

            _player.SetInteractable(_interactable, gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            PlayerInteractionHandler _player = _other.GetComponent<PlayerInteractionHandler>();

            _player.ClearInteractable(gameObject);
        }
    }
}