using UnityEngine;

public class RobotMode : MonoBehaviour, IOperatingMode
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _gravityForce;
    [SerializeField] private int _layerNum = 7;

    private CharacterController _characterController;
    private Camera _camera;

    public void Activity(bool isActive)
    {
        if (isActive)
        {
            Vector3 sizeShpere = new Vector3(0, 0.5f, 0);            
            _characterController.center = sizeShpere;
            _characterController.height = 1.6f;
            _characterController.radius = 0.5f;
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 pos = (new Vector3(direction.x, 0, direction.y) * _speed * Time.deltaTime);

        pos += new Vector3(0, _gravityForce * Time.deltaTime, 0);
        _characterController.Move(pos);
    }

    public void Rotate(Vector3 look)
    {
        _characterController.transform.forward = look - _characterController.transform.position;
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetCharacterController(CharacterController charactercontroller)
    {
        _characterController = charactercontroller;
    }

    public int GetLayer { get => _layerNum; }
}
