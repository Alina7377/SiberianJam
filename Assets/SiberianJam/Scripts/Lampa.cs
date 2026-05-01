using UnityEngine;

public class Lampa : MonoBehaviour, IActivate
{
    [SerializeField] private Material _standartMaterial;
    [SerializeField] private MeshRenderer _render;
    [SerializeField] private Material _activateMaterial;

    public void Activate(GameObject obj, bool isActive)
    {
        if (isActive)
            _render.material = _activateMaterial;
        else
            _render.material = _standartMaterial;

    }
}
