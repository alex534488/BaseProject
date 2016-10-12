using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CCC.Manager;

public class IntroScript : MonoBehaviour
{
    public Image title;
    public AudioClip clip;
    public float endScale = 1.2f;
    public bool musicPersistent;
    [Header("Durations")]
    public float fadeInDelay;
    public float fadeIn;
    public float pause;
    public float fadeOut;
    public float loadDelay;

    void Awake()
    {
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        MasterManager.Sync(Init);
    }

    void Init()
    {
        if (musicPersistent) SoundManager.PlayMusic(clip);
        else SoundManager.Play(clip);

        title.DOFade(1, fadeIn).SetDelay(fadeInDelay);
        title.transform.DOScale(endScale, fadeOut + fadeIn + pause + fadeInDelay);
        title.DOFade(0, fadeOut).SetDelay(pause + fadeIn + fadeInDelay).OnComplete(OnAnimComplete);
    }

    void OnAnimComplete()
    {
        DelayManager.CallTo(Load, loadDelay);
    }

    void Load()
    {
        SceneManager.LoadScene("Main");
    }
}
