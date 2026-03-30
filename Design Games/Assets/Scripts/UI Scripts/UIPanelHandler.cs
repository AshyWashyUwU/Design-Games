using UnityEngine;

public class UIPanelHandler : MonoBehaviour
{
    public UIMenuID _menuID;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}