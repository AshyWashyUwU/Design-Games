using UnityEngine;

public class ComputerInteractionHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private UIMenuID _targetMenu;

    public void Interact(GameObject _interactor)
    {
        if (UIMenuSwapHandler._instance._currentMenu == UIMenuSwapHandler._instance._startingMenu) UIMenuSwapHandler._instance.OpenMenu(_targetMenu);

        else UIMenuSwapHandler._instance.CloseMenu();
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
