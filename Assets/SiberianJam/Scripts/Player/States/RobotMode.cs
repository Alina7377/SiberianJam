using UnityEngine;

public class RobotMode : MonoBehaviour, IOperatingMode
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _gravityForce;

    private CharacterController _characterController;
    private Camera _camera;

    public void Activity(bool isActive)
    {
        if (isActive)
        {
            Vector3 sizeShpere = new Vector3(0, 0.5f, 0);            
            _characterController.center = sizeShpere;
            _characterController.height = 2f;
            _characterController.radius = 0.6f;
        }
    }

    public void Move(Vector2 direction)
    {
        Vector3 pos = (new Vector3(direction.x, 0, direction.y) * _speed * Time.deltaTime);

        pos += new Vector3(0, _gravityForce * Time.deltaTime, 0);
        _characterController.Move(pos);
    }

    public void Rotate(Vector2 look)
    {
        if (_camera == null) return;

        Plane playerPlane = new Plane(Vector3.up, _characterController.transform.position);
        Ray ray = _camera.ScreenPointToRay(look);
        if (!playerPlane.Raycast(ray, out var hitDistance)) return;

        _characterController.transform.forward = ray.GetPoint(hitDistance) - _characterController.transform.position;
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetCharacterController(CharacterController charactercontroller)
    {
        _characterController = charactercontroller;
    }
}
