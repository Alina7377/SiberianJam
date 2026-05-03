using UnityEngine;

public interface IModuleAbility 
{
    public void SetActiveVisual(bool isActive);
    public void Interact(bool isActivate);

    public void DopInteract(bool isActive);
}
