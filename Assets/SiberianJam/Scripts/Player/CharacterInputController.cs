using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterState _stateCharacter;
    [SerializeField] private List<SAbility> _interactAbilitys;
    [SerializeField] private Animator _mainAnomator;

    private IModuleAbility _activeModule;
    private int _activeAbylityNum = -1;
    private PlayerController _inputControl;

    private void Awake()
    {
        _inputControl = new PlayerController();
        _inputControl.Gamplay.ChangeMode.started += context => ChangeMode();
        _inputControl.Gamplay.Interact.started += context => Interact();
        _inputControl.Gamplay.Interact.canceled += context => EndInteract();
        _inputControl.Gamplay.ChangeModul.started += context => ChangeModule();
    }

    private void ChangeModule()
    {       
        if (_stateCharacter.IsShpereMode) return;
        if (_interactAbilitys.Count == 0) return;
        EndInteract();
        if (_activeModule != null)
            _activeModule.SetActiveVisual(false);
        _activeAbylityNum++;
        if (_interactAbilitys.Count <= _activeAbylityNum)
            _activeAbylityNum = 0;
        if (_interactAbilitys[_activeAbylityNum].IsCanUse && _interactAbilitys[_activeAbylityNum].AbilityObject is IModuleAbility activeModule)
        {
            Debug.Log("Попали в активацию");
            _activeModule = activeModule;
            _activeModule.SetActiveVisual(true);
        } 
        else
        {
            Debug.Log("Нет активного модуля");
            _activeAbylityNum--;
            if (_activeAbylityNum<0)
                _activeAbylityNum = _interactAbilitys.Count-1;
        }
    }

    private void Interact()
    {
        if (_stateCharacter.IsShpereMode) return;

        if (_activeModule is IModuleAbility ability)
            ability.Interact(true);
    }

    private void EndInteract()
    {
        if (_activeModule is IModuleAbility ability)
            ability.Interact(false);
    }

    private void ChangeMode()
    {
        EndInteract();
        _stateCharacter.ChangeState();
    }

    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }
    
    private void Update()
    {
        _stateCharacter.Movement(_inputControl.Gamplay.MovementStandart.ReadValue<Vector2>());
        _stateCharacter.MovementShpere(_inputControl.Gamplay.MovementSphere.ReadValue<Vector2>());
        _stateCharacter.Rotation(_inputControl.Gamplay.Look.ReadValue<Vector2>());
    }   
}

[Serializable]
public struct SAbility
{
    public MonoBehaviour AbilityObject;
    public bool IsCanUse;
}
