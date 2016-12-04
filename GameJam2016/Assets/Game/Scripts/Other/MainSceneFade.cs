using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;

public class MainSceneFade : MonoBehaviour
{

    public static MainSceneFade instance;

    List<Tweener> tweens = new List<Tweener>();

    public CanvasGroup[] uiGroup;
    public SpriteRenderer blackCover;
    public VignetteAndChromaticAberration vignette;
    public float vignetting = 0.41f;

    [Header("Durations")]
    public float blackFade = 1;
    public float uiFade = 0.5f;

    void Awake()
    {
        instance = this;
    }

    public void FadeIn(UnityAction onComplete = null)
    {
        ClearTweens();

        foreach (CanvasGroup ui in uiGroup)
        {
            ui.alpha = 0;
            ui.interactable = false;
        }

        vignette.intensity = vignetting;
        vignette.enabled = true;

        blackCover.color = Color.black;

        tweens.Add(DOTween.To(() => vignette.intensity, x => vignette.intensity = x, 0, blackFade)); //Fade vignette

        tweens.Add(blackCover.DOFade(0, blackFade).OnComplete(delegate () //Fade black
        {
            vignette.enabled = false;

            bool isFirst = true;
            foreach (CanvasGroup ui in uiGroup)
            {
                ui.interactable = true;

                tweens.Add(ui.DOFade(1, uiFade).OnComplete(delegate () //Fade UI
                {
                    if (isFirst && onComplete != null) onComplete();

                    isFirst = false;
                }));
            }
        }));
    }

    public void FadeOut(UnityAction onComplete = null)
    {
        ClearTweens();

        foreach (CanvasGroup ui in uiGroup)
        {
            ui.alpha = 1;
            ui.interactable = true;
        }

        vignette.intensity = 0;
        vignette.enabled = true;

        blackCover.color = new Color(0, 0, 0, 0);


        bool isFirst = true;
        foreach (CanvasGroup ui in uiGroup)
            tweens.Add(ui.DOFade(0, uiFade).OnComplete(delegate () //Fade UI
            {
                ui.interactable = false;

                if (isFirst)
                {
                    tweens.Add(DOTween.To(() => vignette.intensity, x => vignette.intensity = x, vignetting, blackFade)); //Fade vignette
                    tweens.Add(blackCover.DOFade(1, blackFade).OnComplete(delegate () //Fade black
                    {
                        if (onComplete != null) onComplete();
                    }));
                }
                isFirst = false;
            }));
    }

    void ClearTweens()
    {
        foreach (Tweener tween in tweens) tween.Kill();

        tweens.Clear();
    }
}
