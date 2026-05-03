using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour, ITake
{
    [SerializeField] private CharacterState _stateCharacter;
    [SerializeField] private List<SAbility> _interactAbilitys;
    [SerializeField] private Animator _mainAnomator;

    private IModuleAbility _activeModule;
    private int _activeAbylityNum = 0;
    private PlayerController _inputControl;
    private bool _canUseInteractMode = true;
    private List<GameObject> _volumes = new List<GameObject>();

    private void Awake()
    {
        _inputControl = new PlayerController();
        _inputControl.Gamplay.ChangeMode.started += context => ChangeMode();
        _inputControl.Gamplay.Interact.started += context => Interact();
        _inputControl.Gamplay.Interact.canceled += context => EndInteract();
        _inputControl.Gamplay.ChangeModul.started += context => ChangeModule();
        _inputControl.Gamplay.DopInteract.started += context => DopInteract();
        _inputControl.Gamplay.DopInteract.canceled += context => DopInteractEnd();
    }

    private void DopInteract()
    {

        if (_stateCharacter.IsShpereMode) return;

        if (_activeModule is IModuleAbility ability)
            ability.DopInteract(true);
    }

    private void DopInteractEnd()
    {
        if (_activeModule is IModuleAbility ability)
            ability.DopInteract(false);
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
            _activeModule = activeModule;
            _activeModule.SetActiveVisual(true);
        } 
        else
        {
            _activeAbylityNum--;
            if (_activeAbylityNum<0)
                _activeAbylityNum = _interactAbilitys.Count-1;
            if (!_stateCharacter.IsShpereMode)
            {
                if (_interactAbilitys[_activeAbylityNum].IsCanUse && _interactAbilitys[_activeAbylityNum].AbilityObject is IModuleAbility ability)
                {
                    _activeModule = ability;
                    _activeModule.SetActiveVisual(true);
                }
            }
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
        if (_activeModule == null) return;
        if (_stateCharacter.IsShpereMode && !_canUseInteractMode)
            return;
        EndInteract();
        if (_activeModule != null)
            _activeModule.SetActiveVisual(false);

        _stateCharacter.ChangeState();
        
        if (!_stateCharacter.IsShpereMode && _activeModule!=null)
            _activeModule.SetActiveVisual(true);

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

    public void Take(int num)
    {
        if (num < _interactAbilitys.Count)
        {
            SAbility ability = _interactAbilitys[num];
            ability.IsCanUse = true;
            _interactAbilitys[num] = ability;
        }
        else
            return;
        if (_interactAbilitys[num].AbilityObject is IModuleAbility moduleAbility)
        {
            if (_activeModule != null)
                _activeModule.SetActiveVisual(false);
            _activeModule = moduleAbility;
            _activeAbylityNum = num;
        }

        if (!_stateCharacter.IsShpereMode && _activeModule != null)
            _activeModule.SetActiveVisual(true);
    }

    public void SetCanUseInteractionMode(bool canUse, GameObject volumeObject)
    {
        if (!canUse)
            _volumes.Add(volumeObject);
        else
            _volumes.Remove(volumeObject);
        if (canUse && _volumes.Count == 0)
            _canUseInteractMode = true;
        else
            _canUseInteractMode = false;

    }
}

[Serializable]
public struct SAbility
{
    public MonoBehaviour AbilityObject;
    public bool IsCanUse;
}
