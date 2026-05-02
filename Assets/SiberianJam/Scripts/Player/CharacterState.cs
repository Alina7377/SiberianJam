using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private CharacterController _characterControler;
    [SerializeField] private Animator _mainAnimator;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<MonoBehaviour> _statesScripts;
    [SerializeField] private bool _sphereMode;

    private List<IOperatingMode> _stateMods = new List<IOperatingMode>();
    private bool _isShpereMode = true;


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
        _isShpereMode = _sphereMode;
    }

    public void Movement(Vector2 direction)
    {
        if (!_isShpereMode && _stateMods.Count > 1)
            _stateMods[1].Move(direction);
        else
        {
            if (_stateMods.Count <= 1)
                Debug.Log("Некорректен режим работы робота");
        }
    }
    public void MovementShpere(Vector2 direction) 
    {
        if(_isShpereMode && _stateMods.Count > 0)
            _stateMods[0].Move(direction);
        else
        {
            if ( _stateMods.Count == 0)
                Debug.Log("Некорректен режим работы робота");
        }
    } 

    public void Rotation(Vector2 look)
    {
        if (_isShpereMode)

            _stateMods[0].Rotate(look);

        else
        {
            _stateMods[1].Rotate(look);
        }
    }

    public void ChangeState()
    {
        if(_isShpereMode)
             _stateMods[0].Activity(false);
         else
             _stateMods[1].Activity(false);

         _isShpereMode = !_isShpereMode;
        _mainAnimator.SetBool("IsInteractMode", !_isShpereMode);

        if (_isShpereMode)
            _stateMods[0].Activity(true);
        else
            _stateMods[1].Activity(true);
    }

    public bool IsShpereMode { get => _isShpereMode; }

}
