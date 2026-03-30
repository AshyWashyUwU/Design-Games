using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    private IInteractable _currentInteractable;
    private GameObject _currentObject;

    public void SetInteractable(IInteractable interactable, GameObject obj)
    {
        _currentInteractable = interactable;
        _currentObject = obj;

        if (_currentInteractable != null)
        {
            InteractUIHandler._instance.UpdateInteractedObject(obj, _currentInteractable.GetInteractText(), _currentInteractable.GetInteractOffset());
        }
    }

    public void ClearInteractable(GameObject obj)
    {
        if (_currentObject == obj)
        {
            _currentInteractable = null;
            _currentObject = null;

            InteractUIHandler._instance.UpdateInteractedObject(obj, null, new Vector3(0, 0, 0));
        }
    }

    private void Update()
    {
        if (_currentInteractable != null && PlayerInputManager.interactIsPressed)
        {
            _currentInteractable.Interact(gameObject);
        }
    }
}