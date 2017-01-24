using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using CCC.Manager;

public class DayManager : MonoBehaviour
{

    public static DayManager main;

    // Audio
    public AudioClip loseClip;

    // Boutton du UI
    public Button nextDayButton;

    // Manager et Universe
    public RequestManager requestManager;
    public StorylineManager storylineManager;
    [System.NonSerialized]
    Universe universe;
    public Universe Universe { get { return universe; } }

    // Mode
    public Mode currentMode;

    // Nombre de jours (Points de la partie)
    //public int nbJour = 0;

    private UnityEvent onNewDayTransition = new UnityEvent();
    private UnityEvent onNewDay = new UnityEvent();
    static public UnityEvent OnNewDayTransition
    {
        get { return main != null ? main.onNewDayTransition : null; }
    }
    static public UnityEvent OnNewDay
    {
        get { return main != null ? main.onNewDay : null; }
    }

    void Awake()
    {
        main = this;

        MasterManager.Sync();
    }

    void Start()
    {
        if (ModeManager.modeManager != null)
            currentMode = ModeManager.modeManager.GetCurrentMode();

        // Initialisation du système de requête pour la première journée
        requestManager.onAllRequestsComplete.AddListener(OnAllRequestComplete);

        // Permet de passer au prochain jour
        if (nextDayButton != null) nextDayButton.onClick.AddListener(OnNextDayClick);

        MainSceneFade.instance.FadeIn(OnFadeInComplete);
    }

    public void Init(GameSave save = null)
    {
        if (save != null)
            universe = new Universe(save.world, null);
        else
            universe = new Universe();
    }

    public void OnFadeInComplete()
    {
        if (universe != null)
        {
            onNewDayTransition.Invoke();
            FirstDay();
        }
        else
            Debug.LogWarning("Cannot proceed to next day because the universe is null");
    }

    void OnNextDayClick()
    {
        onNewDayTransition.Invoke();

        // Desactive les boutons temporairement
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = false;

        DayOfTime.Night();
        DelayManager.CallTo(delegate ()
        {
            NewDay();
        }, 1);
    }

    public void FirstDay()
    {
        DayOfTime.Day(0);

        // Desactive les boutons temporairement
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = true; // a changer lorsqu'on aura une requete de depart

        universe.world.empire.FirstDay();
        requestManager.FirstDay();

        onNewDay.Invoke();
    }

    /// <summary>
    /// On passe au prochain jour en s'assurant de tout mettre à jour
    /// </summary>
    public void NewDay()
    {
        DayOfTime.Day(0);

        // Desactive les boutons temporairement
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = false;

        // Update l'univers
        universe.NewDay();
        storylineManager.NewDay();
        requestManager.NewDay();

        onNewDay.Invoke();
    }

    private void OnAllRequestComplete()
    {
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = true;
    }

    /// <summary>
    /// Fin de la partie en allant faire la scene de game over
    /// </summary>
    public void Lose(string reason)
    {
        PlayerPrefs.SetInt("highscore", Universe.World.CurrentDay);

        SoundManager.PlayMusic(loseClip, false, 1, true);
        MainSceneFade.instance.FadeOut(delegate ()
        {
            Scenes.Load("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single, delegate (UnityEngine.SceneManagement.Scene scene)
            {
                scene.GetRootGameObjects()[0].GetComponent<OutroScript>().Init(reason);
            });
        });
    }

    private void ApplyGameMode()
    {

    }
}
