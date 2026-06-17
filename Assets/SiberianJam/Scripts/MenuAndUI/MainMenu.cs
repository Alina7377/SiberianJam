using SaveSystemData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _butContinueGames;
    [SerializeField] private Button _butSelectLevels;

    [Header("Для тестов")]
    [SerializeField] private string _nameStartScene = "Scene_1";
    [SerializeField] private string _nameStartLevel = "Lvl1_5";

    private void Start()
    {
        LoadSettings();        
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("LastLvl"))
        {
            _butContinueGames.interactable  = true;
            _butSelectLevels.interactable = true;
        }
    }

    public void OnNewGame()
    {
        List<string> args = new List<string> { _nameStartScene, _nameStartLevel};
        SceneLoader.Instance.LoadeLevel("Level", args);
    }

    public void OnLoadGame()
    {
        string levelName = PlayerPrefs.GetString("LastLvl");
        SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(levelName));
        List<string> args = new List<string> { saveData.SceneName, saveData.LevelName, saveData.LevelName };
        SceneLoader.Instance.LoadeLevel("Level", args);

    }

    public void OnShowMenuSelectLevel()
    {
 
    }

    public void OnShowSettingMenu()
    {
        Settings.Instance.OnShow();
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
