using UnityEngine;

public class SphereMode : MonoBehaviour, IOperatingMode
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _smoothnesMovement;
    [SerializeField] private Animator _mainAnimator;
    [SerializeField] private float _gravityForce;
    [SerializeField] private int _layerNum = 3;

    private CharacterController _characterController;
    private Camera _camera;

    private Vector3 _mausePosition;
    private Vector3 _currentPos = Vector3.zero;
    private Vector3 _targetPos = Vector3.zero;

    private bool _isActive = true;

    public void Activity(bool isActive)
    {
        _isActive = isActive;
        if (isActive)
        {
            Vector3 sizeShpere = Vector3.zero;
            _characterController.center = sizeShpere;
            _characterController.height = 0.3f;
            _characterController.radius = 0.3f;
        }
        else
        {
            _currentPos = Vector3.zero;
            _targetPos = Vector3.zero;
        }
    }

    private void Update()
    {
        if (_isActive && _characterController.enabled)
            Movement();
    }

    private void Movement()
    {
        _currentPos = Vector3.Lerp(_currentPos, _targetPos, _smoothnesMovement * Time.deltaTime);

        Vector3 pos = _currentPos + new Vector3(0, _gravityForce * Time.deltaTime, 0);

        _characterController.Move(pos);
    }
    public void Move(Vector2 direction)
    {
        float distance = Vector3.Distance(_characterController.transform.position, _mausePosition);
        float speed = (_speed / _maxDistance) * distance;
        int kof = direction.y <= 0 ? 0 : 1;

        _mainAnimator.SetFloat("Speed", (speed / _speed) * kof);
        speed = Mathf.Min(_speed, speed);

        Vector3 animatePos = _characterController.transform.TransformDirection(new Vector3(0, 0, direction.y)).normalized;
        _targetPos = animatePos * speed * Time.deltaTime;
    }

    public void Rotate(Vector2 look)
    {
        if (_camera == null) return;

        Plane playerPlane = new Plane(Vector3.up, _characterController.transform.position);
        Ray ray = _camera.ScreenPointToRay(look);
        if (!playerPlane.Raycast(ray, out var hitDistance))
        {
            _mausePosition = Vector3.zero;
            return;
        }

        _mausePosition = ray.GetPoint(hitDistance);
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

    public int GetLayer { get => _layerNum; }
}
