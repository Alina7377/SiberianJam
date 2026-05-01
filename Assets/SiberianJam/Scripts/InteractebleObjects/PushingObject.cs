using UnityEngine;

public class PushingObject : MonoBehaviour, IPushObject
{
    [SerializeField] private Rigidbody _rigidbody;

    public void Push(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}
