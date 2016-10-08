using UnityEngine;
using System.Collections;

public class DayManager : MonoBehaviour{

    public World theWorld;
    public RequestManager requestManager;

	void Start ()
    {
        requestManager.Start();
	}
	
	public void Update ()
    {
        requestManager.onWaitingForRequest.Invoke();
        
        // Display Bouton Next Day
	}

    public void NextDay()
    {
        theWorld.Update();
    }



}
