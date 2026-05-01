using UnityEngine;

public class DraggingAbility : MonoBehaviour, IModuleAbility
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _radiusHand;

    private GameObject _dragObject = null;
    private bool _isDrag = false;

    private void DragObgect()
    {
        if (_dragObject != null)
        {
            _isDrag = true;
            return;
        }

        Collider[] hitObjects = Physics.OverlapSphere(_handPoint.position, _radiusHand);

        if (hitObjects == null || hitObjects.Length == 0) return;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<IDragObject>(out IDragObject interactObject))
            {
                interactObject.Interact();
                _isDrag = true;
                _dragObject = hitObject.gameObject;
                _dragObject.transform.SetParent(_handPoint);
                _dragObject.transform.localPosition = Vector3.zero;
                return;
            }
        }        
    }

    private void DropObject()
    {
        if (_dragObject == null)
        {
            _isDrag = false;
            return;
        }
        if (_dragObject.TryGetComponent<IDragObject>(out IDragObject interactObject))
            interactObject.Interact();
        _dragObject.transform.SetParent(null);
        _dragObject = null;
        _isDrag = false;

    }

    public void Interact(bool isActive)
    {
        if (!isActive)
            DropObject();
        else
            DragObgect();
    }

}
