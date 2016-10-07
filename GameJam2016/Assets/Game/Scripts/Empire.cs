using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class Empire : IUpdate {

    public int nbVillage = 12;

    List<Village> listVillage = new List<Village>();
    Capitale capitale;

	void Start ()
    {
        for (int i = 0; i < nbVillage; i++)
        {
            listVillage.Add(new Village(this,i)); // le village numero 0 correspond a listVillage[0]
        }
        capitale = new Capitale(this,0);
	}
	
	public void Update ()
    {
        foreach (Village village in listVillage)
        {
            village.Update();
        }
    }

    public void DeleteVillage(Village destroyedVillage)
    {
        foreach(Village village in listVillage)
        {
            if(destroyedVillage == village)
            {
                listVillage.Remove(village);
            }
        }
    }
}
