using UnityEngine;

public class Lampa : MonoBehaviour, IActivate
{
    [SerializeField] private Material _standartMaterial;
    [SerializeField] private MeshRenderer _render;
    [SerializeField] private Material _activateMaterial;
    [SerializeField] private float _timeFlash = 1f;
    [SerializeField] private bool _isUse = false;

    private bool _isActive = false;
    private bool _isActiveMAterial = false;
    private float _currentTime = 0;

    private void Start()
    {
        _currentTime = Time.time;
    }

    private void Update()
    {
        if (_isUse && !_isActive)
        {
            if (Time.time > (_currentTime + _timeFlash))
            {
                if (_isActiveMAterial)
                {
                    _render.material = _standartMaterial;
                    _isActiveMAterial = false;
                }
                else
                {
                    _render.material = _activateMaterial;
                    _isActiveMAterial = true;
                }
                _currentTime = Time.time;
            }
           
        }
    }

    public void Activate(GameObject obj, bool isActive)
    {
        if (isActive)
        {
            _render.material = _activateMaterial;
            _isActiveMAterial = true;
            _isActive = true;
        }
        else
            _isActive = false;

    }
}
