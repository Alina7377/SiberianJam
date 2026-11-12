using System;
using UnityEngine;

public class KeybordAndMauseInput : PlayerInputVar
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _maxDistance;

    private PlayerController _inputControl;
    private CharacterInputController _charactController;
   
    private Vector3 _mausePosition;


    private void Update()
    {
        if (_inputControl == null) return;

        Vector2 movement = Movement(_inputControl.KeybordMause.MovementStandart.ReadValue<Vector2>());
        Vector3 movementSphere = MovementShpere(_inputControl.KeybordMause.MovementSphere.ReadValue<Vector2>());
        Vector3 rotation = Rotation(_inputControl.KeybordMause.Look.ReadValue<Vector2>());
        if (movement != Vector2.zero || movementSphere!= Vector3.zero || rotation !=Vector3.zero)
            _charactController.ReadValues(movement, movementSphere, rotation);
    }

    private Vector3 Rotation(Vector2 rotation)
    {

        if (_camera == null)
        {
            Debug.Log("Êàìåðà íå íàçíà÷åíà!");
            return Vector3.zero;
        }

        Plane playerPlane = new Plane(Vector3.up, _characterController.transform.position);
        Ray ray = _camera.ScreenPointToRay(rotation);
        if (!playerPlane.Raycast(ray, out var hitDistance))
            _mausePosition = Vector3.zero;
        else _mausePosition = ray.GetPoint(hitDistance);

        return _mausePosition;
    }

    private Vector3 MovementShpere(Vector2 direction)
    {
        float distance = Mathf.Min(Vector3.Distance(transform.position, _mausePosition), _maxDistance);

        float coff = distance / _maxDistance;

        Vector3 result = new Vector3(direction.x, direction.y, coff);

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

        _inputControl.KeybordMause.ChangeMode.started += context => ChangeMode();
        _inputControl.KeybordMause.Interact.started += context => Interact();
        _inputControl.KeybordMause.Interact.canceled += context => EndInteract();
        _inputControl.KeybordMause.ChangeModul.started += context => ChangeModule();
        _inputControl.KeybordMause.DopInteract.started += context => DopInteract();
        _inputControl.KeybordMause.DopInteract.canceled += context => DopInteractEnd();
        _inputControl.KeybordMause.EndGame.started += context => ShowPauseMenu();
        _inputControl.KeybordMause.ShowInfoHint.started += context => ShowInfo();
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

    private void ShowInfo()
    {
        _charactController.ShowInfo();
    }
}
