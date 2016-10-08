﻿using UnityEngine;
using System.Collections;

public class DayManager : MonoBehaviour{

    public World theWorld;
    public RequestManager requestManager;

	void Start ()
    {
        requestManager.Awake();
	}
	

    public void NextDay()
    {
        requestManager.onWaitingForRequest.Invoke();
        
        // Display Bouton Next Day
        theWorld.Update();
    }
}
