using UnityEngine;

public class ComputerInteractionHandler : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private UIMenuID _computerMenu;

    public void Interact(GameObject _interactor)
    {
        if (UIMenuSwapHandler._instance._currentMenu == UIMenuSwapHandler._instance._startingMenu) UIMenuSwapHandler._instance.OpenMenu(_computerMenu);

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
