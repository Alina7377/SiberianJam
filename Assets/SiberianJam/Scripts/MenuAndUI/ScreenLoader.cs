using UnityEngine;
using UnityEngine.UI;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField] private Canvas _canvasLoader;
    [SerializeField] private Canvas _endGameMenu;

    private CharacterInputController _character;

    private void Start()
    {
        
    }

    public void ShowScreen() 
    {
        _canvasLoader.enabled = true;
        _endGameMenu.enabled = false;
    }

    public void HideScreen()
    {
        _canvasLoader.enabled = false;
        _endGameMenu.enabled = false;
    }

    public void ShowEndGame()
    {
        _endGameMenu.enabled = true;
        if (_character == null)
            _character = FindAnyObjectByType<CharacterInputController>();
        _character.ActiveControl(false);
        
    }

    public void OnReturnMainMenu()
    {
        if (SceneLoader.Instance == null) return;
        SceneLoader.Instance.LoadeLevel("MainMenu", null);
    }
}
