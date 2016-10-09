using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Music : MonoBehaviour {

    // Sounds
    List<AudioClip> listMusic = new List<AudioClip>();
    public AudioClip music1;
    public AudioClip music2;
    private AudioClip nextsong;
    // rajouter des musiques ICI
    public SoundManager soundmanager;

    // Use this for initialization
    void Start()
    {
        listMusic.Add(music1);
        listMusic.Add(music2);
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
