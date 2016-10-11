using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Music : MonoBehaviour
{

    // Sounds
    public List<AudioClip> listMusicVictoire = new List<AudioClip>();
    public List<AudioClip> listMusicDefaite = new List<AudioClip>();
    
    private bool hasInit = false;

    void Awake()
    {
        MasterManager.Sync(Init);

        if (listMusicDefaite.Count <= 1 || listMusicVictoire.Count <= 1)
        {
            Debug.LogError("There must be at least 2 victorious clips and 2 losing clips.");
            enabled = false;
        }
    }

    void Init()
    {
        hasInit = true;
    }

    void Update()
    {
        if (!hasInit) return;

        if (!SoundManager.IsPlayingMusic()) // Si aucune musique ne joue, en lancer une
        {
            PlayNewMusic();
        }
    }

    void PlayNewMusic()
    {
        if (EstimationEmpire.Estimation() > 0.60f) // Somme nous victorieux ?
        {
            PlayMusicFrom(listMusicVictoire);
        }
        else // Somme nous entrain de perdre ?
        {
            PlayMusicFrom(listMusicDefaite);
        }
    }

    void PlayMusicFrom(List<AudioClip> list)
    {
        int index = Random.Range(0, list.Count - 1); //Choisi un clip dans la liste (EXCLUANT LE DERNIER CLIP)
        AudioClip pickedClip = list[index];

        SoundManager.PlayMusic(pickedClip, false);

        list[index] = list[list.Count - 1]; //Met le dernier clip à la position du clip qu'on vient juste de choisir
        list[list.Count-1] = pickedClip; //Met le clip qu'on vient juste de choisir à la dernière position (il ne sera pas choisie la prochaine fois)
    }

}
