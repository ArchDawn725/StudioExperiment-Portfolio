using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SttingsMenuManager : MonoBehaviour
    
{
    public Slider MasterVolumeControl, MusicVolumeControl, SFXVolumeControl;
    public AudioMixer NewAudioMixer;

    private void Start()
    {
        MasterVolumeControl.value = PlayerPrefs.GetFloat("MasterVolume", MasterVolumeControl.value);
        MusicVolumeControl.value = PlayerPrefs.GetFloat("MusicVolume", MusicVolumeControl.value);
        SFXVolumeControl.value = PlayerPrefs.GetFloat("SFXVolume", SFXVolumeControl.value);
    }

    public void ChangeMasterVolume()
    {
        NewAudioMixer.SetFloat("MasterVolumeControl", MasterVolumeControl.value);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeControl.value);
    }
    public void ChangeMusicVolume()
    {
        bool checker = NewAudioMixer.SetFloat("MusicVolumeControl", MusicVolumeControl.value);
        if (!checker) { Debug.LogError("Cannot find music volume control external variable"); }
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeControl.value);
    }
    public void ChangeSFXVolume()
    {
        bool checker = NewAudioMixer.SetFloat("SFXVolumeControl", SFXVolumeControl.value);
        if (!checker) { Debug.LogError("Cannot find SFX volume control external variable"); }
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeControl.value);
    }


}
