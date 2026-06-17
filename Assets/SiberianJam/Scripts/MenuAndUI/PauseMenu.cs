using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CharacterInputController _characterControl;
    [SerializeField] private Canvas _pauseMenuCanvas;

    private void Start()
    {
        if (_characterControl != null)
            _characterControl.SetPauseMenu(this);
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

    public void OnReturnMainMenu()
    {
        if (SceneLoader.Instance == null) return;
        SceneLoader.Instance.LoadeLevel("MainMenu", null);
    }
}
