using System;
using UnityEngine;

public class PushingAbility : MonoBehaviour, IModuleAbility
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private Vector3 _sizeBox;

    [SerializeField] private float _forcePush;

    private bool _isPusshing = false;

    public void Interact(bool active)
    {
        _isPusshing = active;
    }

    private void FixedUpdate()
    {
        if (!_isPusshing) return;

        Push();
    }

    private void Push()
    {
        Debug.Log("Ïóø");
        Collider[] hitObjects = Physics.OverlapBox(_handPoint.position, _sizeBox);

        if (hitObjects == null || hitObjects.Length == 0) return;
        foreach (var hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<IPushObject>(out IPushObject pushObject))
            {
                pushObject.Push(transform.forward, _forcePush);                
                return;
            }
        }
    }
}
