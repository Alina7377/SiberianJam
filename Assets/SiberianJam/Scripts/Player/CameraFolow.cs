using SaveSystemData;
using System.Collections;
using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    [SerializeField] float _smoothness;
    [SerializeField] private Transform _targetObject;
    private Vector3 _initalOffset;
    private Vector3 _cameraPosition;

    private float _currentSmoothness = 2.5f;

    private void Start()
    {
        _initalOffset = transform.position - _targetObject.position;
    }

    private void LateUpdate()
    {
        if (_targetObject == null)
            return;
        _cameraPosition = _targetObject.position + _initalOffset;
        transform.position = Vector3.Lerp(transform.position, _cameraPosition, _currentSmoothness * Time.deltaTime);
    }

    public void SetPlayingSmoothnes(bool isPlayengMode)
    {
        if (isPlayengMode)
            _currentSmoothness = _smoothness;
        else
            _currentSmoothness = 50f;
    }
}
