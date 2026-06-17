using SaveSystemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObject : SavedObject, IPushObject
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ETypeObject _type;
    [SerializeField] private float _offsetY = 0.371f;

    public ETypeObject GetObjectType => _type;
    private bool _isPush = true;
   // private Vector3 _defautState = new Vector3(0.)

    private void Start()
    {
        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }

    private void FixedUpdate()
    {
        if (_isPush)
        {
            //_rigidbody.constraints = RigidbodyConstraints.None;
            StartCoroutine(DiactivePhysics());
        }

    }

    private IEnumerator DiactivePhysics()
    {
        _isPush = false;
        yield return new WaitForSeconds(0.5f);
        if (_rigidbody.linearVelocity.x == 0 && _rigidbody.linearVelocity.y == 0 && _rigidbody.linearVelocity.z == 0)
        {
            _rigidbody.isKinematic = true;
        }
        else
        {
            _isPush = true;
            StartCoroutine(DiactivePhysics());
        }
    }

    public void Push(Vector3 direction, float force)
    {

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        _isPush = true;
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
        Vector3 loadPosition = loadData.VectorPatameters[0];
        loadPosition.y += _offsetY;
        _rigidbody.isKinematic = true;
        transform.position = loadPosition;
        transform.rotation = loadData.Rotate;
        _rigidbody.isKinematic = false;
    }
}
