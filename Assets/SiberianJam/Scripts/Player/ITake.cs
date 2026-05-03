using UnityEngine;

public interface ITake 
{
    public void Take(int num);

    public void SetCanUseInteractionMode(bool canUse, GameObject volume);
}
