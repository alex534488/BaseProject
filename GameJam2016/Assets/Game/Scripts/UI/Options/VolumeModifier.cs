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
        if(newValue <= -20)
        {
            SoundManager.SetMusic(-80);
        } else
        {
            SoundManager.SetMusic(newValue);
        }
    }

    public void OnEffetVolumeChange(float newValue)
    {
        if (newValue <= -20)
        {
            SoundManager.SetSfx(-80);
        }
        else
        {
            SoundManager.SetSfx(newValue);
        }
    }

    public void Mute()
    {
        float value = slider.GetComponent<Slider>().value;
        if (value > -20)
        {
            if (slider.name == "Sfx")
            {
                PlayerPrefs.SetFloat("sfx", slider.GetComponent<Slider>().value);
                SoundManager.SetSfx(-80);
                slider.GetComponent<Slider>().value = -20;
            }
            else if (slider.name == "Music")
            {
                PlayerPrefs.SetFloat("music", slider.GetComponent<Slider>().value);
                SoundManager.SetMusic(-80);
                slider.GetComponent<Slider>().value = -20;
            }
        } else
        {
            if (slider.name == "Sfx")
            {
                float newValue = slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sfx");
                SoundManager.SetSfx(newValue);
            }
            else if (slider.name == "Music")
            {
                float newValue = slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("music");
                SoundManager.SetMusic(newValue);
            }  
        }
    }
}
