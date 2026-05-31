using UnityEngine;

public class UpgradesGuyInteractionHandler : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private UIMenuID _upgradesMenu;

    public void Interact(GameObject _interactor)
    {
        if (UIMenuSwapHandler._instance._currentMenu == UIMenuSwapHandler._instance._startingMenu) UIMenuSwapHandler._instance.OpenMenu(_upgradesMenu);

        else UIMenuSwapHandler._instance.CloseMenu();
    }

    public string GetInteractText()
    {
        return "Change Upgrades";
    }

    public Vector3 GetInteractOffset()
    {
        return new Vector3(0, 3, 0);
    }
}
