using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayManager : MonoBehaviour{

    public static DayManager main;

    public Button nextDayButton;
    public World theWorld;
    public RequestManager requestManager;

    public int nbJour = 0;

    void Awake()
    {
        if (main == null) main = this;

        theWorld = new World();
        theWorld.Start();

        // INTRODUCTION

        requestManager.OnCompletionOfRequests.AddListener(OnAllRequestComplete);
        nextDayButton.onClick.AddListener(NextDay);
    }

    public void NextDay()
    {
        nbJour++;
        print("JOUR " + nbJour);

        theWorld.Update(); // Update le monde
        
        // Desactive les boutons temporairement
        nextDayButton.GetComponent<Button>().interactable = false;

        // Debute la phase des requetes
        PhaseRequete(); 

    }

    public void PhaseRequete()
    {
        requestManager.NewDay(); // Affiche toutes les requetes
    }

    private void OnAllRequestComplete()
    {
        nextDayButton.GetComponent<Button>().interactable = true;
    }
}
