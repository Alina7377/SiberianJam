using UnityEngine;

public class NoUseIntecatMode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Попал предмет " + other);
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
            takeComponent.SetCanUseInteractionMode(false, this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ушел предмет предмет " + other);
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
            takeComponent.SetCanUseInteractionMode(true, this.gameObject);
    }
}
