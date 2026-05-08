using UnityEngine;

public class DraggingAbility : MonoBehaviour, IModuleAbility
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _radiusHand;
    [SerializeField] private GameObject _visualComponent;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _newRadius;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audio;
    [SerializeField] private LayerMask _layerMaskChrackHit;

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
                _audioSource.PlayOneShot(_audio);
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

    private bool IsCanDrag()
    {
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        float distance = Vector3.Distance(transform.position, _handPoint.position) + (_radiusHand / 2);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMaskChrackHit))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Interact(bool isActive)
    {
        if (!isActive)
            DropObject();
        else
            if (IsCanDrag())
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
