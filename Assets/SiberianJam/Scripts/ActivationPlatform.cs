using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivationPlatform : MonoBehaviour, IActivate
{
    [SerializeField] private Material _standartMaterial;
    [SerializeField] private ETypeObject _typeActivateObject;
    [SerializeField] private List<SActivateInput> _inputSignals;
    [SerializeField] private List<SActivateOutput> _outputSignals;

    private bool _isCanActivate = false;
    private bool _hasRequiredObject = false;

    private void Start()
    {
        if (_inputSignals.Count == 0)
            _isCanActivate = true;
    }

    private void OnTriggerEnter(Collider collision)
    {        
        if (collision.gameObject.TryGetComponent<IInteractObject>(out IInteractObject interactObject))
            if (interactObject.GetObjectType == _typeActivateObject)
            {
                _hasRequiredObject = true;
                if (!_isCanActivate) return;
                ChangeState(true);
            }
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IInteractObject>(out IInteractObject interactObject))
            if (interactObject.GetObjectType == _typeActivateObject)
            {
                _hasRequiredObject = false;
                if (!_isCanActivate) return;
                ChangeState(false);
            }
    }

    private void ChangeState(bool isActive)
    {
        foreach (var activateSignal in _outputSignals)
        {
            foreach (var visual in activateSignal._visualObjects)
            {
                if (isActive)
                    visual.material = activateSignal._material;
                else
                    visual.material = _standartMaterial;
            }
            
            foreach (var activateObject in activateSignal._activateObjects)
            {
                if (activateObject is IActivate activate)
                    activate.Activate(gameObject, isActive);
            }
        }
    }

    public void Activate(GameObject obj, bool isActive)
    {
        bool isAllActive = true;
        for (int i = 0; i < _inputSignals.Count; i++)
        {
            if (_inputSignals[i]._activateObject == obj)
            {
                SActivateInput signal = _inputSignals[i];
                signal._isActive = isActive;
                _inputSignals[i] = signal;
            }
            if (!_inputSignals[i]._isActive)
                isAllActive = false;
        }
        _isCanActivate = isAllActive;
        if (_hasRequiredObject)
            ChangeState(_isCanActivate);
    }
}

[Serializable]
public struct SActivateInput 
{
    public GameObject _activateObject;
    public bool _isActive;
}

[Serializable]
public struct SActivateOutput
{
    public Material _material;
    public List<MeshRenderer> _visualObjects;
    public List<MonoBehaviour> _activateObjects;
}
