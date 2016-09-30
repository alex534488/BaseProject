using UnityEngine;
using System.Collections;
using CCC.Manager;

public class QuickSound : MonoBehaviour {

    public float delay = 0;
    public float volume = 0;
    public AudioClip clip;
    public AudioSource source;

    void Awake()
    {
        MasterManager.Sync(Sync);
    }

    void Sync()
    {
        MasterManager.master.GetManager<SoundManager>().Play(clip, delay, volume, source);
    }
}
