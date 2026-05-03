using UnityEngine;

public class NoUseIntecatMode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
            takeComponent.SetCanUseInteractionMode(false, this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
            takeComponent.SetCanUseInteractionMode(true, this.gameObject);
    }
}
