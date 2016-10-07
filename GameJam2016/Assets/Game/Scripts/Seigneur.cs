using UnityEngine;
using System.Collections;

public class Seigneur : IUpdate {

    public Village village;

    // Seuil de tolerance permis par le seigneur
    private int seuilNourriture;
    private int seuilGold;
    private int seuilArmy;

    public Seigneur(Village village)
    {
        this.village = village;
        seuilNourriture = village.nourrirPopulation;
        seuilGold = village.coutNourriture;
        //seuilArmy = A determiner; 
    }

	void Start ()
    {
	
	}
	
	public void Update ()
    {
        if (village.isDestroyed) Death();
        
        if (village.nourriture < seuilNourriture) NeedFood();
        if (village.or < seuilGold) NeedGold();
        //if (village.army < seuilArmy) NeedArmy();
    }

    void Death()
    {
        village.DestructionVillage();
        // this meurt
    }

    void NeedFood()
    {

    }

    void NeedGold()
    {

    }

    void NeedArmy()
    {

    }
}
