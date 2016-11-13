using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour {

    public GameObject changer;

	public void OnVolumeChange(float newValue)
    {
        // TODO: Changer le SoundManager pour avoir un seul volume constant que l'on peut changer par newValue
        //SoundManager
    }

    public void OnMusicToggle(bool newBool)
    {

    }
}
