using System.Collections.Generic;
using UnityEngine;

public class UIMenuSwapHandler : MonoBehaviour
{
    private static UIMenuSwapHandler Instance;

    public static UIMenuSwapHandler _instance { get => Instance; }

    private Dictionary<UIMenuID, UIPanelHandler> _menus = new();
    private Stack<UIPanelHandler> _menuHistory = new();

    [SerializeField] private UIMenuID _startingMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        UIPanelHandler[] _panels = FindObjectsByType<UIPanelHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var _panel in _panels)
        {
            if (!_menus.ContainsKey(_panel._menuID))
            {
                _menus.Add(_panel._menuID, _panel);
            }
            _panel.Hide();
        }

        OpenMenu(_startingMenu);
    }

    public void OpenMenu(UIMenuID _newMenuID)
    {
        UIPanelHandler _nextMenu = _menus[_newMenuID];

        if (_menuHistory.Count > 0) _menuHistory.Peek().Hide();

        _nextMenu.Show();
        _menuHistory.Push(_nextMenu);
    }

    public void BackMenu()
    {
        if (_menuHistory.Count <= 1) return;

        _menuHistory.Pop().Hide();
        _menuHistory.Peek().Show();
    }
}