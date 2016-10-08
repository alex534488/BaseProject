using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayManager : MonoBehaviour{

    public static DayManager main;

    public Button nextDayButton;
    public World theWorld;
    public RequestManager requestManager;

    void Awake()
    {
        if (main == null) main = this;
        nextDayButton.onClick.AddListener(NextDay);
    }

    public void NextDay()
    {
        print("Next day!");
        requestManager.NewDay();
        
        // Display Bouton Next Day
        theWorld.Update();
    }
}
