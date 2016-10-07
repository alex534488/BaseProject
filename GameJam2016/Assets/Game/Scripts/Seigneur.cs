using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Seigneur : IUpdate {

    public Village village;

    // Seuil de tolerance permis par le seigneur
    private int seuilNourriture;
    private int seuilGold; // or minimale permis, correspond au coutNourriture de village
    private int seuilArmy;

    public Seigneur(Village village)
    {
        this.village = village;
        seuilNourriture = village.nourrirPopulation;
        seuilGold = village.coutNourriture;
        seuilArmy = 10; 
    }
	
	public void Update ()
    {
        if (village.isDestroyed) Death();

        if (village.isAttacked)
        {

        }
        
        if (village.nourriture < seuilNourriture) NeedFood();
        if (village.or < seuilGold) NeedGold();
        if (village.army < seuilArmy) NeedArmy(seuilArmy - village.army);
    }

    void Death()
    {
        village.DestructionVillage();
        // DO: this meurt
    }

    void NeedFood()
    {
        if (village.or < seuilGold) NeedGold();
        village.or -= village.coutNourriture;
        village.nourriture += village.nourrirPopulation;
    }

    void NeedGold()
    {
        // va voir la capitale pour de l'or
    }

    void NeedArmy(int amount)
    {
        if (village.or < village.coutArmy) NeedGold();
        village.or -= village.coutArmy * amount;
        village.army += amount;
    }
}
