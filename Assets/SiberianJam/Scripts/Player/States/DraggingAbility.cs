using UnityEngine;

public class DraggingAbility : MonoBehaviour, IModuleAbility
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _radiusHand;
    [SerializeField] private GameObject _visualComponent;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _newRadius;

    private GameObject _dragObject = null;
    private float _baseSize = 0;

    private void DragObgect()
    {
        if (_dragObject != null)
        {
            return;
        }

        Collider[] hitObjects = Physics.OverlapSphere(_handPoint.position, _radiusHand);

        if (hitObjects == null || hitObjects.Length == 0) return;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<IDragObject>(out IDragObject interactObject))
            {
                _baseSize = _characterController.radius;
                interactObject.Interact();
                _dragObject = hitObject.gameObject;
                _dragObject.transform.SetParent(_handPoint);
                _dragObject.transform.localPosition = Vector3.zero;
                _characterController.radius = _newRadius;
                return;
            }
        }        
    }

    private void DropObject()
    {
        if (_dragObject == null)
        {
            return;
        }
        if (_baseSize > 0)
            _characterController.radius = _baseSize;

        if (_dragObject.TryGetComponent<IDragObject>(out IDragObject interactObject))
            interactObject.Interact();
        
        _dragObject.transform.SetParent(null);
        _dragObject = null;

    }

    public void Interact(bool isActive)
    {
        if (!isActive)
            DropObject();
        else
            DragObgect();
    }

    public void SetActiveVisual(bool isActive)
    {
        _visualComponent.SetActive(isActive);
    }

    public void DopInteract(bool isActive)
    {
        Interact(isActive);
    }
}
