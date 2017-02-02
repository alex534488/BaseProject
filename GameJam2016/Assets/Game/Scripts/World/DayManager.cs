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

    private UnityEvent onNewDayTransition = new UnityEvent();       // Début de la nuit
    private UnityEvent onNewDay = new UnityEvent();                 // Début du jour
    private UnityEvent onArrival = new UnityEvent();                // Arrivé du joueur dans le jour
    private UnityEvent onInit = new UnityEvent();                   // Dès que le DayManager initialize le monde
    private bool isInDayTransition = false;
    static public UnityEvent OnNewDayTransition
    {
        get { return main != null ? main.onNewDayTransition : null; }
    }
    static public UnityEvent OnNewDay
    {
        get { return main != null ? main.onNewDay : null; }
    }
    static public UnityEvent OnArrival
    {
        get { return main != null ? main.onArrival : null; }
    }
    static public bool IsTransitionningToNewDay { get { return main.isInDayTransition; } }
    static public void SyncToUniverseInit (UnityAction action)
    {
        if (main == null)
            return;
        if (main.universe != null)
            action();
        else
            main.onInit.AddListener(action);
    }

    void Awake()
    {
        universe = null;
        main = this;

        MasterManager.Sync();
    }

    void Start()
    {
        if (ModeManager.modeManager != null)
            currentMode = ModeManager.modeManager.GetCurrentMode();

        MainSceneFade.instance.FadeIn(OnFadeInComplete);
    }

    public void Init(GameSave save = null)
    {
        if (save != null)
        {
            universe = new Universe(save.currentWorld, save.history);
            RequestManager.ApplyMailBox(save.currentMailBox);
            StorylineManager.ApplySaveState(save.currentStorylines);
        }
        else
        {
            universe = new Universe();
            universe.history.RecordDay();
        }
        universe.history.OnPastLoaded.AddListener(ArrivalDay);

        onInit.Invoke();
    }

    public void OnFadeInComplete()
    {
        if (universe != null)
        {
            ArrivalDay();
        }
        else
            Debug.LogWarning("Cannot proceed to next day because the universe is null");
    }

    public void OnNextDayClick()
    {
        isInDayTransition = true;
        onNewDayTransition.Invoke();

        DayOfTime.Night();
        DelayManager.CallTo(delegate ()
        {
            NewDay();
        }, 1);
    }

    //Est appelé lorsque le joueur entre dans le milieu d'une journé (chargement d'une sauvegarde, rewind dans History)
    public void ArrivalDay()
    {
        isInDayTransition = false;
        DayOfTime.Day(0);

        universe.ArrivalDay();
        requestManager.ArrivalDay();
        storylineManager.ArrivalDay();

        onArrival.Invoke();
    }

    /// <summary>
    /// On passe au prochain jour en s'assurant de tout mettre à jour
    /// </summary>
    public void NewDay()
    {
        isInDayTransition = false;
        DayOfTime.Day(0);

        // Update l'univers
        universe.NewDay();
        storylineManager.NewDay();
        requestManager.NewDay();

        universe.history.RecordDay();

        //C'est important de laisser le 'onNewDay' apres les 'core components'
        onNewDay.Invoke();
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

    void OnDestroy()
    {
        main = null;
        Universe.instance = null;
    }

    private void ApplyGameMode()
    {

    }
}
