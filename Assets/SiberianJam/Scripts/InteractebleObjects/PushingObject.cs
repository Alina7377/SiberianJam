using UnityEngine;

public class PushingObject : MonoBehaviour, IPushObject
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ETypeObject _type;

    public ETypeObject GetObjectType => _type;

    public void Push(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}
