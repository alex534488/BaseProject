using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public int nbJour = 0;

    void Awake()
    {
        if (main == null) main = this;

        theWorld = new World();
        theWorld.Start(barbareManager);

        // INTRODUCTION

        requestManager.OnCompletionOfRequests.AddListener(OnAllRequestComplete);
        if (scoutButton != null) nextDayButton.onClick.AddListener(LaunchedDay);
        if(scoutButton != null) scoutButton.onClick.AddListener(ButtonScout);
        if (sendcarriage != null) sendcarriage.onClick.AddListener(Test);
    }

    public void LaunchedDay()
    {
        nbJour++;
        if(currentday != null) currentday.GetComponentInChildren<Text>().text = "Jour " + nbJour;

        theWorld.Update(); // Update le monde
        carriageManager.NewDay();

        // Desactive les boutons temporairement
        if (scoutButton != null) nextDayButton.GetComponent<Button>().interactable = false;
        if (scoutButton != null) scoutButton.GetComponent<Button>().interactable = false;
        if (sendcarriage != null) sendcarriage.GetComponent<Button>().interactable = false;

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
        theWorld.empire.capitale.SendScout(theWorld);
    }

    void Test()
    {
        theWorld.empire.capitale.SendCartToVillage(theWorld.empire.listVillage[0], Ressource_Type.gold, 10);
    }
}
