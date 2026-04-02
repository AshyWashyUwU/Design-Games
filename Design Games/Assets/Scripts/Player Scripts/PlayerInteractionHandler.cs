using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    private IInteractable _currentInteractable;
    private GameObject _currentObject;

    public void SetInteractable(IInteractable _interactable, GameObject _interactableObject)
    {
        _currentInteractable = _interactable;
        _currentObject = _interactableObject;

        if (_currentInteractable != null)
        {
            InteractUIHandler._instance.UpdateInteractedObject(_interactableObject, _currentInteractable.GetInteractText(), _currentInteractable.GetInteractOffset());
        }
    }

    public void ClearInteractable(GameObject _interactableObject)
    {
        if (_currentObject == _interactableObject)
        {
            _currentInteractable = null;
            _currentObject = null;

            InteractUIHandler._instance.UpdateInteractedObject(_interactableObject, null, new Vector3(0, 0, 0));
        }
    }

    private void Update()
    {
        if (_currentInteractable != null && PlayerInputManager._interactIsPressed)
        {
            _currentInteractable.Interact(gameObject);
        }
    }
}