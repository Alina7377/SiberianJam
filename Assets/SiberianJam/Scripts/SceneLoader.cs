using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{    
    [SerializeField] private ScreenLoader _screenLoader;
    public static SceneLoader Instance;

    private CharacterController _playerPers;
    private string _removeScene = string.Empty;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);        
    }

    private IEnumerator LoadAsync(string nameScene, List<string> args)
    {
        ShowLoadingScreen();

        AsyncOperation waitLoadScene = SceneManager.LoadSceneAsync(nameScene);
        yield return new WaitUntil(() => waitLoadScene.isDone);

        if (args != null)
        {
            StartCoroutine(AddLevel(args));
        }
        else
            HideLoadingScreen();

    }

    public void LoadeLevel(string nameScene, List<string> args)
    {
        _removeScene = string.Empty;
        StartCoroutine(LoadAsync(nameScene, args));
    }


    public IEnumerator AddLevel(List<string> args)
    {
        ShowLoadingScreen();

        AsyncOperation waitLoadScene = SceneManager.LoadSceneAsync(args[0], LoadSceneMode.Additive);
        yield return new WaitUntil(() => waitLoadScene.isDone);

        if (_removeScene != string.Empty)
            StartCoroutine(RemoveLevel(_removeScene));
        else
            if (SaveSystem.Instance != null)
            SaveSystem.Instance.UpdateSaveObjectsList();

        _removeScene = args[0];

        if (_playerPers == null)
            _playerPers = FindAnyObjectByType<CharacterController>();

        SceneBootstrap sceneBoots = FindAnyObjectByType<SceneBootstrap>();
        if (sceneBoots != null)
            sceneBoots.LoadArgs(args, _playerPers);
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.SetSceneName(_removeScene);

        HideLoadingScreen();
    }

    private IEnumerator RemoveLevel(string sceneName)
    {
        AsyncOperation waitLoadScene = SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitUntil(() => waitLoadScene.isDone);
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.UpdateSaveObjectsList();
    }

    public void ShowLoadingScreen()
    {
        _screenLoader.ShowScreen();
    }

    public void ShowEndGameSreen()
    {
        _screenLoader.ShowEndGame();
    }

    public void HideLoadingScreen()
    {
        _screenLoader.HideScreen();
    }
}
