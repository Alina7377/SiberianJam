using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _musiks;

    private int _currentMusik = -1;

    private void Update()
    {
        if (IsClipEnded())
            PlayeNextMusic();
    }

    private bool IsClipEnded()
    {
        if (_audioSource.clip == null)
            return true;
        if (_audioSource.time >= _audioSource.clip.length - 1)
            return true;
        return false;
    }

    private void PlayeNextMusic()
    {
        _audioSource.Stop();
        _currentMusik++;
        if (_currentMusik >= _musiks.Count)
            _currentMusik = 0;
        _audioSource.clip = _musiks[_currentMusik];
        _audioSource.Play();
    }

    private void PlayRandomMusik()
    {
        int indexMusik = UnityEngine.Random.Range(0, _musiks.Count);
        _audioSource.clip = _musiks[indexMusik];
        _audioSource.Play();
    }
}
