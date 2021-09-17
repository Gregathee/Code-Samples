using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adjusts the volume of game audio based on slider positions
/// </summary>
public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    
    public void UpdateVolumeValues()
    {
        AudioManager.instance.musicVolume = _musicSlider.value;
        AudioManager.instance.sfxVolume = _sfxSlider.value;
    }
}