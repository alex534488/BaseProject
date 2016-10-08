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

        GoAskEmperor("Nourriture", foodneeded);

        if (village.or < goldneed) NeedGold(goldneed);
        village.or -= goldneed;
        village.nourriture += foodneeded;
    }

    void NeedGold(int amount)
    {
        if (!alreadyAsk)
        {
            GoAskEmperor("Or", -1);
        }
    }

    void NeedArmy(int amount)
    {
        GoAskEmperor("Army", amount);

        if (village.or < village.costArmy) NeedGold(village.costArmy);
        village.or -= village.costArmy * amount;
        village.army += amount;
    }

    void GoAskEmperor(string resource, int amount)
    {
        switch (resource)
        {
            case "Nourriture":
                // envoie un messager a l'emperor pour lui signaler qu'il a besoin de bouffes
            case "Or":
                // envoie un messager a l'emperor pour lui signaler qu'il a besoin d'or
            case "Army":
                // envoie un messager a l'emperor pour lui signaler qu'il a besoin d'argent
            default:
                return;
        }
    }
}
