using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterState _stateCharacter;

    [SerializeField] private List<MonoBehaviour> _interactAbilitys;

    private MonoBehaviour _activeModule;
    private int _activeAbylityNum = 0;
    private PlayerController _inputControl;

    private void Awake()
    {
        _inputControl = new PlayerController();
        _inputControl.Gamplay.ChangeMode.started += context => ChangeMode();
        _inputControl.Gamplay.Interact.started += context => Interact();
        _inputControl.Gamplay.Interact.canceled += context => EndInteract();
        _inputControl.Gamplay.ChangeModul.started += context => ChangeModule();
        if (_interactAbilitys.Count > 0)
            _activeModule = _interactAbilitys[_activeAbylityNum];
    }

    private void ChangeModule()
    {
        EndInteract();
        if (_stateCharacter.IsShpereMode) return;
        if (_interactAbilitys.Count == 0) return;
        Debug.Log("Переклчюение режима");
        _activeAbylityNum++;
        if (_interactAbilitys.Count <= _activeAbylityNum)
            _activeAbylityNum = 0;
        _activeModule = _interactAbilitys[_activeAbylityNum];
        Debug.Log("Новый режим " + _activeAbylityNum);
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
