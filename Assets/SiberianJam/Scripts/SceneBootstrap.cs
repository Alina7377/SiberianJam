using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public struct SSpawnPoint
{
    public string Name;
    public Transform Point;
}

public class SceneBootstrap : MonoBehaviour, ISceneBoost
{
    [SerializeField] private AudioSource _audioEnviroment;
    [SerializeField] private List<SSpawnPoint> _savePoints;    

    public void LoadArgs(List<string> args, CharacterController playerPers)
    {
        if (_savePoints.Count == 0)
        {
            Debug.LogError("На сцене отсутсвуют точки спавна");
            return;
        }

        playerPers.enabled = false;

        SSpawnPoint pointS = _savePoints[0];

        if (args.Count > 1 && args[1] != string.Empty)
            foreach(var point in _savePoints)
            {
                if (args[1] != point.Name) continue;
                pointS = point;
                break;
            }

        // Если этот аргумент есть, то загружаем данные
        if (args.Count > 2 && args[2] != string.Empty && SaveSystem.Instance != null)
            SaveSystem.Instance.LoadData(args[2]);

        playerPers.transform.position = pointS.Point.position;
        playerPers.enabled = true;
        if (_audioEnviroment != null)
            _audioEnviroment.Play();

        playerPers.enabled = true;
    }
}
