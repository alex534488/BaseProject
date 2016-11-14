using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour {

	public void OnMusiqueVolumeChange(float newValue)
    {
        SoundManager.SetMusic(newValue);
    }

    public void OnEffetVolumeChange(float newValue)
    {
        SoundManager.SetSfx(newValue);
    }
}
