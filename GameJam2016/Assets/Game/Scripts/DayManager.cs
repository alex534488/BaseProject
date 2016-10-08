using UnityEngine;
using System.Collections;

public class DayManager : MonoBehaviour{

    public static DayManager main;

    public World theWorld;
    public RequestManager requestManager;

    void Awake()
    {
        if (main == null) main = this;
    }

    public void NextDay()
    {
        requestManager.NewDay();
        
        // Display Bouton Next Day
        theWorld.Update();
    }
}
