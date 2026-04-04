using System.Collections.Generic;
using UnityEngine;

public class UIMenuSwapHandler : MonoBehaviour
{
    private static UIMenuSwapHandler Instance;

    public static UIMenuSwapHandler _instance { get => Instance; }

    private Dictionary<UIMenuID, UIPanelHandler> _menus = new();
    private Stack<UIPanelHandler> _menuHistory = new();

    public UIMenuID _startingMenu { get; private set; }

    public UIMenuID _currentMenu { get; private set; }

    [SerializeField] private GameObject _backgroundBlur;

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
        _currentMenu = _newMenuID;

        UIPanelHandler _nextMenu = _menus[_newMenuID];

        if (_menuHistory.Count > 0) _menuHistory.Peek().Hide();

        _nextMenu.Show();
        _menuHistory.Push(_nextMenu);

        CheckImmobilization(_nextMenu);
    }

    public void BackMenu()
    {
        if (_menuHistory.Count <= 1) return;

        _menuHistory.Pop().Hide();
        _menuHistory.Peek().Show();

        CheckImmobilization(_menuHistory.Peek());
    }

    public void CloseMenu()
    {
        _menuHistory.Pop().Hide();
        _menuHistory.Clear();

        OpenMenu(_startingMenu);
    }

    private void CheckImmobilization(UIPanelHandler _nextMenu)
    {
        if (_nextMenu._playerCanMove == PlayerMovementHandler._instance._immobilized)
        {
            PlayerMovementHandler._instance.ToggleImmobilization();

            _backgroundBlur.SetActive(!_backgroundBlur.activeSelf);
        }
    }
}