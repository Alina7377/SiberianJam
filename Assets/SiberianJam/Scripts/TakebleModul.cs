using UnityEngine;

public class TakebleModul : MonoBehaviour
{
    [SerializeField] private int _moduleNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITake>(out ITake takeComponent))
        {
            takeComponent.Take(_moduleNum);
            Destroy(gameObject);
        }
    }
}
