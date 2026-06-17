using SaveSystemData;
using System.Collections.Generic;
using UnityEngine;

public class DraggingObject : SavedObject, IDragObject
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ETypeObject _type;
    [SerializeField] private float _offsetY = 0.371f;

    public ETypeObject GetObjectType => _type;

    private void Start()
    {
        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }

    public void Interact()
    {
        _rigidbody.isKinematic = !_rigidbody.isKinematic;
    }

    public override SObjectData SaveData()
    {
        if (gameObject == null) return new SObjectData();

        SObjectData saveData = new SObjectData();

        saveData.SavingObjectID = _guid;
        saveData.VectorPatameters = new List<Vector3> { transform.position }; 
        saveData.Rotate = transform.rotation;

        return saveData;
    }

    public override void LoadData(SObjectData loadData)
    {
        if (loadData.SavingObjectID != _guid) return;

        Vector3 loadPosition = loadData.VectorPatameters[0];
        loadPosition.y += _offsetY;
        _rigidbody.isKinematic = true;
        transform.position = loadPosition;
        transform.rotation = loadData.Rotate;
        _rigidbody.isKinematic = false;
    }
}
