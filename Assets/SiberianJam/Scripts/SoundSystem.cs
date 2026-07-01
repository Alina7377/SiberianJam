using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _musiks;

    private void Update()
    {
        if (!_audioSource.isPlaying)
            PlayRandomMusik();
    }

    private void PlayRandomMusik()
    {
        int indexMusik = UnityEngine.Random.Range(0, _musiks.Count);
        _audioSource.clip = _musiks[indexMusik];
        _audioSource.Play();
    }
}
