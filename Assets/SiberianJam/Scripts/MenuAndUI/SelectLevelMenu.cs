using SaveSystemData;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelMenu: MonoBehaviour
{
    [SerializeField] private Canvas _canvasSelectLevel;
    [SerializeField] private GameObject _buttonPref;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private int _levelCount;

    private bool _isShow = false;
    private List<GameObject> _selectionButtons = new List<GameObject>();    

    private void CreateButtons()
    {
        ClearButtons();

        GameObject firstButton = Instantiate(_buttonPref, _buttonContainer);
        SelectLevelButton firstSelectButtons = firstButton.GetComponent<SelectLevelButton>();
        firstSelectButtons.SetParameters(true, "Scene_1", "Lvl1_5", "1-5");
        _selectionButtons.Add(firstButton);

        for (int i = 6; i <= _levelCount; i++)
        {
            GameObject newButton = Instantiate(_buttonPref, _buttonContainer);
            SelectLevelButton selectButton = newButton.GetComponent<SelectLevelButton>();
            string sceneName = string.Empty;
            string levelName = string.Empty;
            bool isLvlComplite = false;
            if (PlayerPrefs.HasKey("Lvl" + i.ToString()))
            {
                isLvlComplite = true;
                SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("Lvl" + i.ToString()));
                sceneName = saveData.SceneName;
                levelName = saveData.LevelName;
            }
            selectButton.SetParameters(isLvlComplite, sceneName, levelName, i.ToString());
            _selectionButtons.Add(newButton);
        }

    }

    private void ClearButtons()
    {
        for (int i = _selectionButtons.Count - 1; i >= 0; i--)
        {
            Destroy(_selectionButtons[i]);
            _selectionButtons.RemoveAt(i);
        }
    }

    public void OnShowSelectLevelMenu()
    {
        _isShow = !_isShow;
        _canvasSelectLevel.enabled = _isShow;
        if (_isShow)
            CreateButtons();
    }
}
