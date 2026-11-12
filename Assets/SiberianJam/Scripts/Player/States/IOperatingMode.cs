using UnityEngine;

public interface IOperatingMode 
{
    public void Move(Vector3 direction);

    public void Rotate(Vector3 look);

    public void SetCharacterController(CharacterController charactercontroller);

    public void SetCamera(Camera camera);

    public void Activity(bool isActive);
}
