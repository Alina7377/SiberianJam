using UnityEngine;

public interface IInteractObject 
{
    public ETypeObject GetObjectType { get; }
}

public enum ETypeObject
{
    Drag,
    Push
}
