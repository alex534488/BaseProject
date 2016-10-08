using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Seigneur : IUpdate {

    public Village village;

    public string nom;

    // Seuil de tolerance permis par le seigneur
    private int seuilNourriture;
    private int seuilGold; // or minimale permis, correspond au coutNourriture de village
    private int seuilArmy;

    private bool alreadyAsk = false;

    public Seigneur(Village village)
    {
        this.village = village;
        seuilNourriture = village.nourrirPopulation;
        seuilGold = village.coutNourriture;
        seuilArmy = 0; 
    }
	
	public void Update ()
    {
        if (village.isDestroyed) Death();

        if (village.isAttacked)
        {
            seuilArmy = village.barbares.nbBarbares;
            int incertitude = Mathf.RoundToInt((seuilArmy/100)*20);
            seuilArmy += Random.Range(-incertitude, incertitude+1);
        }

        seuilNourriture = village.nourrirPopulation + village.nourrirArmy;

        if (village.nourriture < seuilNourriture) NeedFood();
        if (village.or < seuilGold) NeedGold(seuilGold);
        if (village.army < seuilArmy) NeedArmy(seuilArmy - village.army);
    }

    void Death()
    {
        village.DestructionVillage();
        // DO: this meurt
    }

    void NeedFood()
    {
        int foodneeded = village.nourrirPopulation + village.nourrirArmy;
        int goldneed = seuilGold*Mathf.RoundToInt(foodneeded/village.coutNourriture);

        GoAskEmperor(Ressource_Type.food, foodneeded);

        if (village.or < goldneed) NeedGold(goldneed);
        village.or -= goldneed;
        village.nourriture += foodneeded;
    }

    void NeedGold(int amount)
    {
        if (!alreadyAsk)
        {
            GoAskEmperor(Ressource_Type.gold, -1);
        }
    }

    void NeedArmy(int amount)
    {
        GoAskEmperor(Ressource_Type.army, amount);

        if (village.or < village.costArmy) NeedGold(village.costArmy);
        village.or -= village.costArmy * amount;
        village.army += amount;
    }

    void GoAskEmperor(Ressource_Type resource, int amount)
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                RequestManager.SendRequest(new Request(resource,amount));
                return;
            case Ressource_Type.food:
                RequestManager.SendRequest(new Request(resource, amount));
                return;
            case Ressource_Type.army:
                RequestManager.SendRequest(new Request(resource, amount));
                return;
            default:
                return;
        }
    }
}
