using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private Text _outTextButton;
    [SerializeField] private Button _button;
    [SerializeField] private string _sceneName;
    [SerializeField] private string _levelName;

    public void OnSelect()
    {
        List<string> args = new List<string> { _sceneName, _levelName, _levelName };
        SceneLoader.Instance.LoadeLevel("Level", args);
    }

    public void SetParameters(bool isInteract, string sceneName, string levelName, string numlevel)
    {
        _outTextButton.text = numlevel;
        _button.interactable = isInteract;
        _sceneName = sceneName;
        _levelName = levelName;
    }
}
