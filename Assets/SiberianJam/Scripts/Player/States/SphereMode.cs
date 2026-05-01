using UnityEngine;

public class SphereMode : MonoBehaviour, IOperatingMode
{    
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _maxDistance;
    [SerializeField] private GameObject _visualmodel;
    [SerializeField] private float _gravityForce;

    private CharacterController _characterController;
    private Camera _camera;

    private Vector3 _mausePosition;

     

    public void Activity(bool isActive)
    {
        _visualmodel.SetActive(isActive);
        if (isActive)
        {
            Vector3 sizeShpere= Vector3.zero;
            _characterController.center = sizeShpere;
            _characterController.height = 0.5f;
        }
    }

    public void Move(Vector2 direction)
    {
        float distance = Vector3.Distance(_characterController.transform.position, _mausePosition);
        float speed = (_speed / _maxDistance) * distance;

        speed = Mathf.Min(_speed, speed);

        Vector3 animatePos = _characterController.transform.TransformDirection(new Vector3(0, 0, direction.y)).normalized;
        Vector3 pos = animatePos * speed * Time.deltaTime;

        // Получаем приобразование мировых координат в локальные для определения нужной анимации
        /* Vector3 animatePos = _controller.transform.InverseTransformDirection(new Vector3(direction.x, 0, direction.y)).normalized;

         _animationManager.AnimatorSetParameter("Horizontal", animatePos.x);
         _animationManager.AnimatorSetParameter("Vertical", animatePos.z);*/
        pos += new Vector3(0, _gravityForce * Time.deltaTime, 0);
        _characterController.Move(pos);
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
}
