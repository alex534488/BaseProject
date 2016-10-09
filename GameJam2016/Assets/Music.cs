using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Music : MonoBehaviour {

    static Music musicmanager;

    // Sounds
    public List<AudioClip> listMusicVictoire = new List<AudioClip>();
    public List<AudioClip> listMusicDefaite = new List<AudioClip>();
    private AudioClip nextsong;
    // rajouter des musiques ICI

    private double etat = 1;
    
    void Awake()
    {
        if (musicmanager == null) musicmanager = this;
    }

    void Start()
    {
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
