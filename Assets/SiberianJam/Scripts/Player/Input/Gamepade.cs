using System;
using UnityEngine;

public class Gamepade : PlayerInputVar
{
    [SerializeField] private CharacterController _characterController;

    private PlayerController _inputControl;
    private CharacterInputController _charactController;
   
    private Vector3 _mausePosition;
    private Vector2 _direction;


    private void Update()
    {
        if (_inputControl == null) return;

        Vector2 movement = Movement(_inputControl.Gamepade.Movement.ReadValue<Vector2>());
        Vector3 movementSphere = MovementShpere(_inputControl.Gamepade.MovementSphere.ReadValue<float>());
        Vector3 rotation = Rotation(_inputControl.Gamepade.Look.ReadValue<Vector2>());
        if (movement != Vector2.zero || movementSphere != Vector3.zero || rotation != Vector3.zero)
            _charactController.ReadValues(movement, movementSphere, rotation);
    }

    private Vector3 Rotation(Vector2 rotation)
    {
        _direction = rotation;
        Debug.Log(rotation);
        Vector3 result = transform.position + new Vector3(rotation.x, 0, rotation.y);
        return result;
    }

    private Vector3 MovementShpere(float value)
    {
        Vector3 result = new Vector3(_direction.x, _direction.y, value);

        return result;
    }

    private Vector2 Movement(Vector2 direction)
    {
        return direction;
    }

    public override void ÑonfigureInput(PlayerController inputController, CharacterInputController controller)
    {
        base.ÑonfigureInput(inputController, controller);

        _charactController = controller;
        _inputControl = inputController;

        _inputControl.Gamepade.ChangeMode.started += context => ChangeMode();
        _inputControl.Gamepade.Interact.started += context => Interact();
        _inputControl.Gamepade.Interact.canceled += context => EndInteract();
        _inputControl.Gamepade.ChangeModul.started += context => ChangeModule();
        _inputControl.Gamepade.DopInteract.started += context => DopInteract();
        _inputControl.Gamepade.DopInteract.canceled += context => DopInteractEnd();
        _inputControl.Gamepade.EndGame.started += context => ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        _charactController.ShowPauseMenu();
    }

    private void DopInteractEnd()
    {
        _charactController.DopInteract(false);
    }

    private void DopInteract()
    {
        _charactController.DopInteract(true);
    }

    private void ChangeModule()
    {
        _charactController.ChangeModule();
    }

    private void EndInteract()
    {
        _charactController.Interact(false);
    }

    private void Interact()
    {
        _charactController.Interact(true);
    }

    private void ChangeMode()
    {
        _charactController.ChangeMode();
    }
}
