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
    [SerializeField] private int _newLayer = 12;
    [SerializeField] private LayerMask _layerMaskChrackHit;

    private GameObject _dragObject = null;
    private float _baseSize = 0;
    private int _startLayer; 

    private void DragObgect()
    {
        if (_dragObject != null)
        {
            return;
        }
        
        Collider[] hitObjects = Physics.OverlapBox(_handPoint.position, new Vector3(_radiusHand, 3f, _radiusHand),Quaternion.identity);
        //Collider[] hitObjects = Physics.OverlapSphere(_handPoint.position, _radiusHand);

        if (hitObjects == null || hitObjects.Length == 0) return;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<IDragObject>(out IDragObject interactObject))
            {
                _audioSource.PlayOneShot(_audio);
                _baseSize = _characterController.radius;

                _dragObject = hitObject.gameObject;
                _startLayer = _dragObject.layer;
                _dragObject.layer = _newLayer;

                interactObject.Interact(_handPoint);
               
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
            interactObject.Interact(null);
        
        _dragObject.layer = _startLayer;
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
