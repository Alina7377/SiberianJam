using SaveSystemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObject : SavedObject, IPushObject
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private ETypeObject _type;
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _offsetY = 0.371f;

    public ETypeObject GetObjectType => _type;

    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }

    private void Update()
    {
        _direction.y = _gravityForce;
        _controller.Move(_direction * Time.deltaTime);
        _direction = Vector3.zero;
    }

    public void Push(Vector3 direction, float force)
    {
        _direction = direction;
    }

    public override SObjectData SaveData()
    {
        SObjectData saveData = new SObjectData();

        saveData.SavingObjectID = _guid;
        saveData.VectorPatameters = new List<Vector3> { transform.position };
        saveData.Rotate = transform.rotation;

        return saveData;
    }

    public override void LoadData(SObjectData loadData)
    {
        if (loadData.SavingObjectID != _guid) return;
        _controller.enabled = false;
        Vector3 loadPosition = loadData.VectorPatameters[0];
        loadPosition.y += _offsetY;
        transform.position = loadPosition;
        transform.rotation = loadData.Rotate;
        _controller.enabled = true;
    }
}
