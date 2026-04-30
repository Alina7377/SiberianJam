using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private CharacterController _characterControler;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<MonoBehaviour> _statesScripts;
    [SerializeField] private int _startState = 0;

    private List<IOperatingMode> _stateMods = new List<IOperatingMode>();
    private int _currentState;


    private void Start()
    {
        foreach (var state in _statesScripts)
        {
            if (state is IOperatingMode mode)
            {
                mode.SetCharacterController(_characterControler);
                mode.SetCamera(_camera);
                _stateMods.Add(mode);
            }
        }
        _currentState = _startState;
    }

    public void Movement(Vector2 direction)
    {
        if (_currentState >= 0 && _currentState < _stateMods.Count)
            _stateMods[_currentState].Move(direction);
        else
        {
            Debug.Log("Некорректен режим работы робота");
        }
    }

    public void Rotation(Vector2 look)
    {
        if (_currentState >= 0 && _currentState < _stateMods.Count)
            _stateMods[_currentState].Rotate(look);
        else
        {
            Debug.Log("Некорректен режим работы робота");
        }
    }

    public void ChangeState()
    {
        _stateMods[_currentState].Activity(false);
        _currentState++;
        if (_currentState >= _stateMods.Count)
            _currentState = 0;
        _stateMods[_currentState].Activity(true);
    }
}
