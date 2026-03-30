using UnityEngine;

public class ComputerInteractionHandler : MonoBehaviour, IInteractable
{
    public void Interact(GameObject _interactor)
    {

    }

    public string GetInteractText()
    {
        return "Open Computer";
    }

    public Vector3 GetInteractOffset()
    {
        return new Vector3(0, 3, 0);
    }
}
