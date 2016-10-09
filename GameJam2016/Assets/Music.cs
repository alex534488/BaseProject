using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Music : MonoBehaviour {

    static Music musicmanager;

    // Sounds
    List<AudioClip> listMusicVictoire = new List<AudioClip>();
    List<AudioClip> listMusicDefaite = new List<AudioClip>();
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioClip music4;
    public AudioClip music5;
    public AudioClip music6;
    private AudioClip nextsong;
    // rajouter des musiques ICI

    private double etat = 1;
    
    void Awake()
    {
        if (musicmanager == null) musicmanager = this;
    }

    void Start()
    {
        listMusicVictoire.Add(music1);
        listMusicVictoire.Add(music2);
        listMusicVictoire.Add(music3);

        listMusicDefaite.Add(music4);
        listMusicDefaite.Add(music5);
        listMusicDefaite.Add(music6);

        nextsong = listMusicVictoire[(int)Random.Range(0, listMusicVictoire.Count)];
        this.GetComponent<AudioSource>().clip = nextsong;
        this.GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying) return;

        if (EstimationEmpire.Estimation() > 0.60f)
        {
            while (this.GetComponent<AudioSource>().clip == nextsong)
            {
                nextsong = listMusicVictoire[(int)Random.Range(0, listMusicVictoire.Count)];
            }
        } else
        {
            while (this.GetComponent<AudioSource>().clip == nextsong)
            {
                nextsong = listMusicDefaite[(int)Random.Range(0, listMusicDefaite.Count)];
            }
        }

        this.GetComponent<AudioSource>().clip = nextsong;
        this.GetComponent<AudioSource>().Play();
    }

}
