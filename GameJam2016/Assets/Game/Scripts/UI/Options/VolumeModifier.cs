using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour {

    public GameObject slider;

    void Awake()
    {
        if(slider.name == "Sfx")
        {
            slider.GetComponent<Slider>().value = SoundManager.GetSfx();

        } else if (slider.name == "Music")
        {
            slider.GetComponent<Slider>().value = SoundManager.GetMusic();
        }
    }

	public void OnMusiqueVolumeChange(float newValue)
    {
        SoundManager.SetMusic(newValue);
    }

    public void OnEffetVolumeChange(float newValue)
    {
        SoundManager.SetSfx(newValue);
    }
}
