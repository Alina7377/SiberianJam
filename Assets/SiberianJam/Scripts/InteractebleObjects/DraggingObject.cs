using UnityEngine;

public class DraggingObject : MonoBehaviour, IDragObject
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ETypeObject _type;

    public ETypeObject GetObjectType => _type;

    public void Interact()
    {
        _rigidbody.isKinematic = !_rigidbody.isKinematic;
    }
}
