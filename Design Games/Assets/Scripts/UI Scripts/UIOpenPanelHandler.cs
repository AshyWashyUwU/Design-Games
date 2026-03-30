using UnityEngine;

public class UIOpenPanelHandler : MonoBehaviour
{
    [SerializeField] private UIMenuID _targetMenu;

    public void TriggerButton()
    {
        UIMenuSwapHandler._instance.OpenMenu(_targetMenu);
    }
}