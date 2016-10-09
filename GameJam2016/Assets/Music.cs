using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Music : MonoBehaviour {

    // Sounds
    List<AudioClip> listMusicVictoire = new List<AudioClip>();
    List<AudioClip> listMusicDefaite = new List<AudioClip>();
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    private AudioClip nextsong;
    // rajouter des musiques ICI

    // Use this for initialization
    void Start()
    {
        listMusicVictoire.Add(music1);
        listMusicVictoire.Add(music2);
        listMusicVictoire.Add(music3);
        this.GetComponent<AudioSource>().Play();
        nextsong = this.GetComponent<AudioSource>().clip;
    }

    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying) return;

        while (this.GetComponent<AudioSource>().clip == nextsong)
        {
            nextsong = listMusic[(int)Random.Range(0, listMusic.Count)];
        }
        this.GetComponent<AudioSource>().clip = nextsong;
        this.GetComponent<AudioSource>().Play();
    }
}
