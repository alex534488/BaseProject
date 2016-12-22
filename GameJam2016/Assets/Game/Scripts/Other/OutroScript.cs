using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CCC.Manager;

public class OutroScript : MonoBehaviour
{
    public SpriteRenderer title;
    public float endScale = 1.2f;
    public CanvasGroup uiGroup;
    public Text reasonText;
    [Header("Durations")]
    public float fadeIn=1;
    public float uiFadeIn=1;
    public float uiFadeInDelay=1;
    public float scaleIn=4;
    public float fadeOut=1;

    bool hasClicked =false;

    void Awake()
    {
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        uiGroup.alpha = 0;
    }

    public void Init(string reason)
    {
        reasonText.text = reason;

        title.DOFade(1, fadeIn);
        uiGroup.DOFade(1, uiFadeIn).SetDelay(uiFadeInDelay);
        title.transform.DOScale(endScale, fadeIn + scaleIn).SetEase(Ease.OutQuad);
    }

    public void PlayAgain()
    {
        if (hasClicked) return;
        hasClicked = true;

        SoundManager.StopMusic(true);
        uiGroup.DOFade(0, fadeOut);
        title.DOFade(0, fadeOut).OnComplete(Load);
    }

    public void GoToMainMenu()
    {
        if (hasClicked) return;
        hasClicked = true;

        SoundManager.StopMusic(true);
        uiGroup.DOFade(0, fadeOut);
        title.DOFade(0, fadeOut).OnComplete(Menu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Load()
    {
        Scenes.Load("Main");
    }

    void Menu()
    {
        Scenes.Load("MainMenu");
    }
}
