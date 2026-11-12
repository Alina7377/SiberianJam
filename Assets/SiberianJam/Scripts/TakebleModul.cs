using SaveSystemData;
using System.Collections.Generic;
using UnityEngine;

public class TakebleModul : SavedObject
{
    [SerializeField] private int _moduleNum;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Collider _collider;

    private void Start()
    {
        _mesh = gameObject.GetComponent<MeshRenderer>();
        _collider = gameObject.GetComponent<Collider>();

        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }

    public override void LoadData(SObjectData loadData)
    {
        if (loadData.SavingObjectID != _guid) return;

        _mesh.enabled = loadData.BoolParamters[0];
        _collider.enabled = loadData.BoolParamters[1];
    }

    public override SObjectData SaveData()
    {
        SObjectData saveData = new SObjectData();

        saveData.SavingObjectID = _guid;
        saveData.BoolParamters = new List<bool> { _mesh.enabled, _collider.enabled };

        return saveData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
        {
            takeComponent.Take(_moduleNum);
            _mesh.enabled = false;
            _collider.enabled = false;
        }
    }
}
