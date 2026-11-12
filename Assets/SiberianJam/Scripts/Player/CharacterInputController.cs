using SaveSystemData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : SavedObject, ITake
{
    [SerializeField] private InfoMenuControl _infoMenu;
    [SerializeField] private List<PlayerInputVar> _inputVariables;
    [SerializeField] private CharacterState _stateCharacter;
    [SerializeField] private List<SAbility> _interactAbilitys;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audio;

    private IModuleAbility _activeModule;
    private int _activeAbylityNum = 0;
    private PlayerController _inputControl;
    private bool _canUseInteractMode = true;
    private List<GameObject> _volumes = new List<GameObject>();
    private PauseMenu _pauseMenu;

    private void Awake()
    {
        _inputControl = new PlayerController();
        foreach (var input in _inputVariables)
        {
            input.ÑonfigureInput(_inputControl, this);
        }     

        CheackActiveModule();
        if (_guid == string.Empty)
            _guid = CreatHashID(gameObject.name, transform.position);
    }

    public void ShowPauseMenu()
    { 
        if (_pauseMenu!= null)
            _pauseMenu.OnShowPauseMenu(true);
    }

    public void DopInteract(bool isActive)
    {

        if (isActive && _stateCharacter.IsShpereMode) return;

        if (_activeModule is IModuleAbility ability)
            ability.DopInteract(isActive);
    }

    public void ChangeModule()
    {       
        if (_stateCharacter.IsShpereMode) return;
        if (_interactAbilitys.Count == 0) return;
        Interact(false);
        if (_activeModule != null)
            _activeModule.SetActiveVisual(false);
        _activeAbylityNum++;
        if (_interactAbilitys.Count <= _activeAbylityNum)
            _activeAbylityNum = 0;
        if (_interactAbilitys[_activeAbylityNum].IsCanUse)
        {
            SetAbility(_interactAbilitys[_activeAbylityNum].AbilityObject);
            _infoMenu.CurrentModule(_activeAbylityNum);
        } 
        else
        {
            _activeAbylityNum--;
            if (_activeAbylityNum<0)
                _activeAbylityNum = _interactAbilitys.Count-1;
            if (!_stateCharacter.IsShpereMode)
            {
                if (_interactAbilitys[_activeAbylityNum].IsCanUse)
                {
                    SetAbility(_interactAbilitys[_activeAbylityNum].AbilityObject);
                    _infoMenu.CurrentModule(_activeAbylityNum);
                }
            }
        }
    }

    public void Interact(bool isActive)
    {
        if (isActive && _stateCharacter.IsShpereMode) return;

        if (_activeModule is IModuleAbility ability)
            ability.Interact(isActive);
    }

    public void ChangeMode()
    {
        if (_activeModule == null && _stateCharacter.IsShpereMode) return;
        if (_stateCharacter.IsShpereMode && !_canUseInteractMode)
            return;
        Interact(false);
        if (_activeModule != null)
            _activeModule.SetActiveVisual(false);
        
        _audioSource.PlayOneShot(_audio);
        _stateCharacter.ChangeState();
        
        if (!_stateCharacter.IsShpereMode && _activeModule!=null)
            _activeModule.SetActiveVisual(true);
        if (_stateCharacter.IsShpereMode)
        {           
            _infoMenu.CurrentModule(-1);
        }
        else
        {
            _infoMenu.CurrentModule(_activeAbylityNum);
        }

        _infoMenu.CurrentRegime(_stateCharacter.IsShpereMode);
    }

    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }
    
    public void ReadValues(Vector2 movement, Vector3 movementSphere, Vector3 rotation)
    {
        _stateCharacter.Movement(movement);
        _stateCharacter.MovementShpere(movementSphere);
        _stateCharacter.Rotation(rotation);
    }

    private void CheackActiveModule()
    {
        if (_activeModule != null) return;
        int ind = -1;
        foreach (var ability in _interactAbilitys)
        {
            ind++;
            if (ability.IsCanUse)
            {
                SetAbility(ability.AbilityObject);
                _infoMenu.CurrentModule(ind);
                Debug.Log("Àêòèâðèóåì ìîäóëü");
                return;
            }
        }
    }

    private void SetAbility(MonoBehaviour abilityObject)
    {
        if (abilityObject is IModuleAbility activeModule)
        {
            _activeModule = activeModule;
            if (!_stateCharacter.IsShpereMode)
                _activeModule.SetActiveVisual(true);
        }
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

        _infoMenu.CanUseChangeRegim(true, true);

        if (num > 0 && !_stateCharacter.IsShpereMode)
            _infoMenu.CanUseChangeModule(true);

        if (!_stateCharacter.IsShpereMode)
            _infoMenu.CurrentModule(num);
    }

    public void SetCanUseInteractionMode(bool canUse, GameObject volumeObject)
    {
        if (!canUse)
            _volumes.Add(volumeObject);
        else
            _volumes.Remove(volumeObject);

        if (canUse && _volumes.Count == 0)
        { 
            _canUseInteractMode = true;
            _infoMenu.CanUseChangeRegim(true,false);
        }
        else
        {
            _canUseInteractMode = false;
            _infoMenu.CanUseChangeRegim(false,false);
        }

    }

    public override SObjectData SaveData()
    {
        SObjectData saveData = new SObjectData();

        saveData.SavingObjectID = _guid;
        saveData.BoolParamters = new List<bool>();
        
        foreach (var ability in _interactAbilitys)
        {
            saveData.BoolParamters.Add(ability.IsCanUse);           
        }

        return saveData;
    }

    public override void LoadData(SObjectData loadData)
    {
        if (loadData.SavingObjectID != _guid) return;

        if (!_stateCharacter.IsShpereMode)
        {
            _stateCharacter.ChangeState();
            if (_activeModule != null)
                _activeModule.SetActiveVisual(false);
        }

        _activeModule = null;
        _activeAbylityNum = -1;
        _infoMenu.CurrentModule(-1);

        for (int i = 0; i < loadData.BoolParamters.Count; i++)
        {
            SAbility changeAbility = _interactAbilitys[i];            
            changeAbility.IsCanUse = loadData.BoolParamters[i];
            _interactAbilitys[i] = changeAbility;
            if (changeAbility.IsCanUse)
            {
                if (_activeModule == null)
                {
                    _activeAbylityNum = i;
                    if (changeAbility.AbilityObject is IModuleAbility ability)
                        _activeModule = ability;
                }
                _infoMenu.CurrentModule(i);
            }
        }

        _infoMenu.CurrentRegime(_stateCharacter.IsShpereMode);

        if (_activeAbylityNum > -1)
            _infoMenu.CanUseChangeRegim(true, false);
        else
            _infoMenu.CanUseChangeRegim(false, false);


    }

    public void ActiveControl(bool isActive)
    {
        if (isActive)
            _inputControl.Enable();
        else
            _inputControl.Disable();
    }

    public void SetPauseMenu(PauseMenu pauseMenu)
    {
        _pauseMenu = pauseMenu;
    }

    public void ShowInfo()
    {
        _infoMenu.ChangeShow();
    }
}

[Serializable]
public struct SAbility
{
    public MonoBehaviour AbilityObject;
    public bool IsCanUse;
}
