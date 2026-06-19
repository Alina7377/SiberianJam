using SaveSystemData;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : SavedObject
{
    [SerializeField] private Collider _collider;
    [SerializeField] private string _lvlName;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }  

    public override void LoadData(SObjectData loadData)
    {
        if (loadData.SavingObjectID != _guid) return;
        _collider.enabled = loadData.BoolParamters[0];
    }

    public override SObjectData SaveData()
    {
        SObjectData saveData = new SObjectData();

        saveData.SavingObjectID = _guid;
        saveData.BoolParamters = new List<bool> { _collider.enabled };

        return saveData;
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;
        _audioSource.Play();
        SaveSystem.Instance.SavingData(_lvlName);
    }
}
