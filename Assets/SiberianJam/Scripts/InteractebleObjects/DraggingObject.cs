using UnityEngine;

public class DraggingObject : MonoBehaviour, IDragObject
{
    [SerializeField] private Rigidbody _rigidbody;


    public void Interact()
    {
        _rigidbody.isKinematic = !_rigidbody.isKinematic;
    }
}
