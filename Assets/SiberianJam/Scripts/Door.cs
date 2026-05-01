using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivate
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<SActivateInput> _inputSignals;    

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
        OpenDoor(isAllActive);
    }

    private void OpenDoor(bool isOpen)
    {
        _animator.SetBool("isOpen", isOpen);
    }
}
