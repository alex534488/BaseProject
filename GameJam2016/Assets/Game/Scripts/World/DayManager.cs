using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using CCC.Manager;

public class DayManager : MonoBehaviour {

    public static DayManager main;

    public AudioClip loseClip;
    // Boutton du UI
    public Button nextDayButton;
    public GameObject currentday;
    public Button scoutButton;

    // Manager et World
    public World theWorld;
    public RequestManager requestManager;
    public CarriageManager carriageManager;
    public BarbareManager barbareManager;

    // Nombre de jours (Points de la partie)
    public int nbJour = 0;

    void Start()
    {
        main = this;

        MasterManager.Sync(null);

        theWorld = new World();
        theWorld.Start();

        if (currentday != null) currentday.GetComponentInChildren<Text>().text = "Jour " + nbJour;

        requestManager.OnCompletionOfRequests.AddListener(OnAllRequestComplete);
        if (scoutButton != null) nextDayButton.onClick.AddListener(OnNextDayClick);
        if(scoutButton != null) scoutButton.onClick.AddListener(ButtonScout);

        MainSceneFade.instance.FadeIn(Init);
    }

    public void Init()
    {
        RequestManager.SendRequest(new Request());
        LaunchDay();
    }

    void OnNextDayClick()
    {
        // Desactive les boutons temporairement
        if (scoutButton != null) nextDayButton.GetComponent<Button>().interactable = false;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = false;

        DayOfTime.Night();
        DelayManager.CallTo(delegate ()
        {
            DayOfTime.Day(1-EstimationEmpire.Estimation());
            LaunchDay();
        }, 1);
    }

    public void LaunchDay()
    {
        // Desactive les boutons temporairement
        if (scoutButton != null) nextDayButton.GetComponent<Button>().interactable = false;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = false;

        nbJour++;
        if(currentday != null) currentday.GetComponentInChildren<Text>().text = "Jour " + nbJour;

        theWorld.NewDay(); // Update le monde
        carriageManager.NewDay();

        // Debute la phase des requetes
        PhaseRequete(); 
    }

    public void PhaseRequete()
    {
        requestManager.NewDay(); // Affiche toutes les requetes
    }

    private void OnAllRequestComplete()
    {
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = true;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = true;
        if (nextDayButton != null) nextDayButton.GetComponent<Button>().interactable = true;
    }

    void ButtonScout()
    {
        scoutButton.GetComponent<Button>().interactable = false;
        theWorld.empire.capitale.SendScout(theWorld);
    }

    public void Lose(string reason)
    {
        PlayerPrefs.SetInt("highscore", nbJour);

        SoundManager.PlayMusic(loseClip, false, 1, true);
        MainSceneFade.instance.FadeOut(delegate ()
        {
            Scenes.Load("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single, delegate (UnityEngine.SceneManagement.Scene scene)
            {
                scene.GetRootGameObjects()[0].GetComponent<OutroScript>().Init(reason);
            });
        });
    }
}
