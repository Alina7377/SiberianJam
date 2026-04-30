using UnityEngine;

public interface IOperatingMode 
{
    public void Move(Vector2 direction);

    public void Rotate(Vector2 look);

    public void SetCharacterController(CharacterController charactercontroller);

    public void SetCamera(Camera camera);

    public void Activity(bool isActive);
}
