using UnityEngine;
using System.Collections;

public class Seigneur : IUpdate {

    public Village village;

    public Seigneur(Village village)
    {
        this.village = village;
    }

	void Start ()
    {
	
	}
	
	public void Update ()
    {
        if (village.nourriture <= 0)
        {
            //famine
            Death();
        }
    }

    void Death()
    {
        village.DestructionVillage();
        // this meurt
    }
}
