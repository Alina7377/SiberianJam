using System;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterState _stateCharacter;

    [SerializeField] private MonoBehaviour _shootAbility;
    [SerializeField] private MonoBehaviour _uniqueAbility;
    [SerializeField] private MonoBehaviour _inventary;

    private PlayerController _inputControl;

    private void Awake()
    {
        _inputControl = new PlayerController();
        _inputControl.Gamplay.ChangeMode.started += context => ChangeMode();
        _inputControl.Gamplay.Interact.started += context => Interact();
        /*  _inputControl.Gamplay.Recharge.started += context => Recharge();
          _inputControl.Gamplay.Shoot.started += context => OneShoot();
          // Инвенатрь
          _inputControl.Inventary.Slot1.started += context => UseItemInInventory(1);
          _inputControl.Inventary.Slot2.started += context => UseItemInInventory(2);
          _inputControl.Inventary.Slot3.started += context => UseItemInInventory(3);
          _inputControl.Inventary.Slot4.started += context => UseItemInInventory(4);*/
    }

    private void Interact()
    {
        throw new NotImplementedException();
    }

    private void ChangeMode()
    {
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
        _stateCharacter.Movement(_inputControl.Gamplay.Movement.ReadValue<Vector2>());
        _stateCharacter.Rotation(_inputControl.Gamplay.Look.ReadValue<Vector2>());
    }


    private void Shoot()
    {
        /*if (_shootAbility is IShootAbility shoot)
            shoot.Execute();*/
    }

    private void OneShoot()
    {
       /* if (_shootAbility is IOneShootAbility shoot)
            shoot.Execute();*/
    }

    private void UniqueAbility()
    {
       /* if (_uniqueAbility is IAbility ability)
            ability.Execute();*/
    }

    private void Recharge()
    {
       /* if (_shootAbility is IShootAbility shoot)
        {
            shoot.Recharge();
            return;
        }
        if (_shootAbility is IOneShootAbility oneShoot)
            oneShoot.Recharge();*/
    }

    private void UseItemInInventory(int numSlot)
    {
        /*if (_inventary is IInventory inventary)
            inventary.UseItem(numSlot);*/
    }
}
