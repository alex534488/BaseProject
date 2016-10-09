using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CCC.Manager;

public class DayManager : MonoBehaviour{

    public static DayManager main;

    // Boutton du UI
    public Button nextDayButton;
    public GameObject currentday;
    public Button scoutButton;
    public Button sendcarriage;

    public World theWorld;
    public RequestManager requestManager;
    public CarriageManager carriageManager;
    public BarbareManager barbareManager;

    // Nombre de jours (Points de la partie)
    public int nbJour = 0;

    void Awake()
    {
        if (main == null) main = this;

        theWorld = new World();
        theWorld.Start();

        // INTRODUCTION

        requestManager.OnCompletionOfRequests.AddListener(OnAllRequestComplete);
        if (scoutButton != null) nextDayButton.onClick.AddListener(OnNextDayClick);
        if(scoutButton != null) scoutButton.onClick.AddListener(ButtonScout);
        if (sendcarriage != null) sendcarriage.onClick.AddListener(Test);
    }

    void OnNextDayClick()
    {
        // Desactive les boutons temporairement
        if (scoutButton != null) nextDayButton.GetComponent<Button>().interactable = false;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = false;
        if (sendcarriage != null) sendcarriage.GetComponent<Button>().interactable = false;

        DayOfTime.Night();
        DelayManager.CallTo(delegate ()
        {
            DayOfTime.Day(1-EstimationEmpire.Estimation());
            LaunchDay();
        }, 1);
    }

    public void LaunchDay()
    {
        nbJour++;
        if(currentday != null) currentday.GetComponentInChildren<Text>().text = "Jour " + nbJour;

        theWorld.Update(); // Update le monde
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
        if (sendcarriage != null) nextDayButton.GetComponent<Button>().interactable = true;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = true;
        if (sendcarriage != null) sendcarriage.GetComponent<Button>().interactable = true;
    }

    void ButtonScout()
    {
        scoutButton.GetComponent<AudioSource>().Play();
        theWorld.empire.capitale.SendScout(theWorld);
    }

    void Test()
    {
        sendcarriage.GetComponent<AudioSource>().Play();
        theWorld.empire.capitale.SendCartToVillage(theWorld.empire.listVillage[0], Ressource_Type.gold, 10);
    }
}
