using System.Collections.Generic;
using UnityEngine;
using SaveSystemData;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private SceneBootstrap _bootstrap;

    private SavedObject[] _savedObjects;

    public static SaveSystem Instance;

    private string _nameScene;
    private CameraFolow _camera;

    private void Awake()
    {
        Instance = this;
        _camera = FindAnyObjectByType<CameraFolow>();
    }

    public void UpdateSaveObjectsList()
    {
        _savedObjects = FindObjectsByType<SavedObject>();
    }

    public void SavingData(string levelName)
    {
        SaveData saveData = new SaveData();


        /*  string nameLastLvl = levelName;
          if (PlayerPrefs.HasKey("LastLvl"))
          {
              nameLastLvl = PlayerPrefs.GetString("LastLvl");            
          }

          if (nameLastLvl.CompareTo(levelName) < 1)
              PlayerPrefs.SetString("LastLvl", levelName);*/

        PlayerPrefs.SetString("LastLvl", levelName);

        saveData.SceneName = _nameScene;
        saveData.LevelName = levelName;

        List<SObjectData> objectsData = new List<SObjectData>();
        foreach (var sObject in _savedObjects)
        {
            if (sObject.gameObject == null) continue;
            objectsData.Add(sObject.SaveData());
        }

        saveData.ObjectsData = new List<SObjectData> (objectsData);

        string jsonData = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(levelName, jsonData);
    }

    private IEnumerator AudioOn()
    {
        yield return new WaitForSeconds(0.5f);
        if (Settings.Instance != null)
            Settings.Instance.DisableEffect(false);
        if (_camera != null)
            _camera.SetPlayingSmoothnes(true);
    }

    public void LoadData(string levelName)
    {
        if (_camera != null)
            _camera.SetPlayingSmoothnes(false);
        if (Settings.Instance != null)
            Settings.Instance.DisableEffect(true);

        if (!PlayerPrefs.HasKey(levelName)) return;

        string levelData = PlayerPrefs.GetString(levelName);
        SaveData saveData = JsonUtility.FromJson<SaveData>(levelData);

        foreach (var obj in _savedObjects)
            foreach (var dat in saveData.ObjectsData)
            {                
                obj.LoadData(dat);
            }

        StartCoroutine(AudioOn());
        
    }

    public void SetSceneName( string nameScene)
    {
        _nameScene = nameScene;
    }

    public void RestartLevl()
    {
        if (_bootstrap == null) return;
        string levelName = PlayerPrefs.GetString("LastLvl");
        SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(levelName));
        List<string> args = new List<string> { saveData.SceneName, saveData.LevelName, saveData.LevelName };

        if (saveData.SceneName != _nameScene)
        {
            SceneLoader.Instance.LoadeLevel("Level", args);
            return;
        }

        _bootstrap.LoadArgs(args, SceneLoader.Instance.GetPlayerCharacter);
    }
}
