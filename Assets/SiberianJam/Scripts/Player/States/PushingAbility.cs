using System;
using UnityEngine;

public class PushingAbility : MonoBehaviour, IModuleAbility
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private Vector3 _sizeBox;
    [SerializeField] private GameObject _visualComponent;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audio;
    [SerializeField] private LayerMask _layerMaskChrackHit;

    [SerializeField] private float _forcePush;

    private bool _isPusshing = false;
    private float _currentForce;

    public void DopInteract(bool active)
    {
        _audioSource.PlayOneShot(_audio);
        _isPusshing = active;
        _currentForce = _forcePush * -1;
    }

    public void Interact(bool active)
    {
        if (active && !IsCanPuch()) return;
        if (active)
            _audioSource.PlayOneShot(_audio);
        _currentForce = _forcePush;
        _isPusshing = active;
    }

    public void SetActiveVisual(bool isActive)
    {
        _visualComponent.SetActive(isActive);
    }

    private void FixedUpdate()
    {
        if (!_isPusshing) return;

        Push();
    }

    private void Push()
    {
        Collider[] hitObjects = Physics.OverlapBox(_handPoint.position, _sizeBox);

        if (hitObjects == null || hitObjects.Length == 0) return;

        foreach (var hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<IPushObject>(out IPushObject pushObject))
            {
                pushObject.Push(transform.forward, _currentForce);             
                return;
            }
        }
    }

    private bool IsCanPuch()
    {
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        float distance = Vector3.Distance(transform.position, _handPoint.position) + (_sizeBox.z);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMaskChrackHit))
        {
            Debug.Log(hit.collider.gameObject);
            return false;
        }
        else
        {
            return true;
        }
    }
}
