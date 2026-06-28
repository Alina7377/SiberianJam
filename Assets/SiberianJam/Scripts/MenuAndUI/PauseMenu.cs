using SaveSystemData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CharacterInputController _characterControl;
    [SerializeField] private Canvas _pauseMenuCanvas;

    private void Start()
    {
        if (_characterControl != null)
            _characterControl.SetPauseMenu(this);

        if (Settings.Instance != null)
        {
            Settings.Instance.OnOpenSettingMenu += HideMenu;
            Settings.Instance.OnCloseSettingMenu += ShowMenu;
        }
    }

    private void ShowMenu()
    {
        _pauseMenuCanvas.enabled = true;
    }

    private void HideMenu()
    {
        _pauseMenuCanvas.enabled = false;
    }

    private void OnDisable()
    {
        if (Settings.Instance != null)
        {
            Settings.Instance.OnOpenSettingMenu -= HideMenu;
            Settings.Instance.OnCloseSettingMenu -= ShowMenu;
        }
    }

    public void OnShowPauseMenu(bool isShow)
    {
        if (_characterControl == null) return;

        _characterControl.ActiveControl(!isShow);       
        _pauseMenuCanvas.enabled = isShow;
    }

    public void OnShowSettings()
    {
        if (Settings.Instance == null) return;
        Settings.Instance.OnShow();
    }

    public void OnRestartLastLevel()
    {
        if (SaveSystem.Instance == null) return;

        SaveSystem.Instance.RestartLevl();
    }

    public void OnReturnMainMenu()
    {
        if (SceneLoader.Instance == null) return;
        SceneLoader.Instance.LoadeLevel("MainMenu", null);
    }
}
