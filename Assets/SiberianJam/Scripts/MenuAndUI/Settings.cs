using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [SerializeField] private Canvas _settingsScreen;
    [SerializeField] private Slider _effectSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private SoundMaster _soundMaster;

    private const string _effectVolume = "EffectsVolume";
    private const string _musicVolume = "MusicVolume";

    private const float _defaulValue = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CheackSettings();
    }

    private void CheackSettings()
    {
        if (PlayerPrefs.HasKey("EffectVolume"))
            _soundMaster.SetSoundVolume(_effectVolume, PlayerPrefs.GetFloat("EffectVolume"));
        else
            PlayerPrefs.SetFloat("EffectVolume", _soundMaster.GetSoundVolume(_effectVolume));

        if (PlayerPrefs.HasKey("MusicVolume"))
            _soundMaster.SetSoundVolume(_musicVolume, PlayerPrefs.GetFloat("MusicVolume"));
        else
            PlayerPrefs.SetFloat("MusicVolume", _soundMaster.GetSoundVolume(_musicVolume));
    }

    public void OnShow()
    {
        float volumeEff = _soundMaster.GetSoundVolume(_effectVolume);
        float volumeMus = _soundMaster.GetSoundVolume(_musicVolume);

        _settingsScreen.enabled = true;
        _effectSlider.value = volumeEff <= _effectSlider.minValue ? _effectSlider.minValue : volumeEff;
        _musicSlider.value = volumeMus <= _musicSlider.minValue ? _musicSlider.minValue : volumeMus;
    }

    public void OnHide()
    {
        _settingsScreen.enabled = false;
        PlayerPrefs.SetFloat("EffectVolume", _soundMaster.GetSoundVolume(_effectVolume));
        PlayerPrefs.SetFloat("MusicVolume", _soundMaster.GetSoundVolume(_musicVolume));
    }

    public void OnChangeEffectVolume()
    {
        _soundMaster.SetSoundVolume(_effectVolume, _effectSlider.value);
    }

    public void OnChangeMusicVolume()
    {
        _soundMaster.SetSoundVolume(_musicVolume, _musicSlider.value);
    }

    public void OnSetDefaultValue()
    {
        _soundMaster.SetSoundVolume(_effectVolume, _defaulValue);
        _soundMaster.SetSoundVolume(_musicVolume, _defaulValue);
        _effectSlider.value = _defaulValue;
        _musicSlider.value = _defaulValue;
    }

   

}
