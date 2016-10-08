using UnityEngine;
using System.Collections;

public class DayManager : IUpdate {

    public World theWorld;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	public void Update ()
    {


        theWorld.Update();

	}



}
