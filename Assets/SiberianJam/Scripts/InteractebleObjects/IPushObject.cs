using UnityEngine;

public interface IPushObject : IInteractObject
{
    public void Push(Vector3 direction, float force);
}
