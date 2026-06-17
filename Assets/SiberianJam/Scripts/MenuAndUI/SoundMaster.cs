using UnityEngine;
using UnityEngine.Audio;

public class SoundMaster : MonoBehaviour
{
    private const float DisableVolume = -80;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _minValueSlider = -20;

    public void SetSoundVolume(string nameParam, float volume)
    {
        float value;
        if (volume < _minValueSlider)
            value = DisableVolume;
        else
            value = volume;

        _audioMixer.SetFloat(nameParam, value);
    }

    public float GetSoundVolume(string nameParam)
    {
         _audioMixer.GetFloat(nameParam, out float value);
        return value;
    }


}
