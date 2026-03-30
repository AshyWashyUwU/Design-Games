using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);

    string GetInteractText();

    Vector3 GetInteractOffset();
}