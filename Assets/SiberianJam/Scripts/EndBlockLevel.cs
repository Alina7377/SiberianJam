using System.Collections.Generic;
using UnityEngine;

public class EndBlockLevel : MonoBehaviour
{
    [SerializeField] private string _nameNextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController character))
        {
            character.enabled = false;
            if (_nameNextScene == string.Empty)
                SceneLoader.Instance.ShowEndGameSreen();
            else
                StartCoroutine(SceneLoader.Instance.AddLevel(new List<string> { _nameNextScene }));
        }
    }
}
