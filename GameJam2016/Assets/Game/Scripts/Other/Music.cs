using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;
using CCC.Manager;

public class Music : MonoBehaviour
{
    public RandomAudioCliptList victoryMusic = new RandomAudioCliptList();
    public RandomAudioCliptList defeatMusic = new RandomAudioCliptList();

    private bool hasInit = false;

    void Awake()
    {
        MasterManager.Sync(Init);
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
            PlayMusicFrom(victoryMusic);
        }
        else // Somme nous entrain de perdre ?
        {
            PlayMusicFrom(defeatMusic);
        }
    }

    void PlayMusicFrom(RandomList<AudioClip> list)
    {
        SoundManager.PlayMusic(list.Pick(), false);
    }

}
