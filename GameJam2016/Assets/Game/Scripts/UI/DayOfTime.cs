using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class DayOfTime : MonoBehaviour
{
    static DayOfTime main;
    static float duration = 0.45f;
    static List<Tweener> tweens = new List<Tweener>();

    public ScreenOverlay blueFilter;
    float defaultBlue;
    public ScreenOverlay darkFilter;
    float defaultDark;
    public ScreenOverlay redFilter;
    float defaultRed;
    public SpriteRenderer skybox;
    public AudioSource audioSource;
    public AudioClip nightClip;
    public AudioClip dayClip;

    void Awake()
    {
        main = this;
        defaultBlue = blueFilter.intensity;
        defaultDark = darkFilter.intensity;
        defaultRed = redFilter.intensity;
        blueFilter.intensity = 0;
        blueFilter.enabled = true;
        darkFilter.intensity = 0;
        darkFilter.enabled = true;
        redFilter.intensity = 0;
        redFilter.enabled = true;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N)) Night();
    //    if (Input.GetKeyDown(KeyCode.J)) Day(0);
    //}

    static public void Night()
    {
        ClearTweens();
        tweens.Add(DOTween.To(() => main.blueFilter.intensity, x => main.blueFilter.intensity = x, main.defaultBlue, duration));
        tweens.Add(DOTween.To(() => main.darkFilter.intensity, x => main.darkFilter.intensity = x, main.defaultDark, duration));
        tweens.Add(DOTween.To(() => main.redFilter.intensity, x => main.redFilter.intensity = x, 0, duration));
        tweens.Add(main.skybox.DOFade(0, duration));

        if (main.nightClip != null) main.audioSource.PlayOneShot(main.nightClip, 0.5f);
    }

    static public void Day(float intensity)
    {
        print("Ambiant intensity: " + intensity + "%        (0% -> fine     100% ->  deep shit)");

        ClearTweens();
        tweens.Add(DOTween.To(() => main.blueFilter.intensity, x => main.blueFilter.intensity = x, 0, duration));
        tweens.Add(DOTween.To(() => main.darkFilter.intensity, x => main.darkFilter.intensity = x, 0, duration));
        tweens.Add(DOTween.To(() => main.redFilter.intensity, x => main.redFilter.intensity = x, main.defaultRed * intensity, duration));
        tweens.Add(main.skybox.DOFade(1, duration));

        if (main.dayClip != null) main.audioSource.PlayOneShot(main.dayClip);
    }

    static void ClearTweens()
    {
        foreach (Tweener tween in tweens) tween.Kill();

        tweens.Clear();
    }
}


