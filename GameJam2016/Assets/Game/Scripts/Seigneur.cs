﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Seigneur : IUpdate {

    // Le village dirige par le seigneur
    public Village village;

    // Nom du Seigneur
    public string nom;

    // Cout pour envoyer un message a l'Empereur
    public int coutMessager = 10;

    // Seuil de tolerance permis par le seigneur
    private int seuilNourriture;
    private int seuilGold; // or minimale permis, correspond au coutNourriture de village
    public int seuilArmy;

    // Es ce que le seigneur a deja demander a l'emperor
    public bool alreadyAsk = false;

    public Seigneur(Village village)
    {
        this.village = village;
        seuilNourriture = village.nourrirArmy;
        seuilGold = village.coutNourriture * village.nourrirArmy;
        seuilArmy = 0; 
    }
	
	public void Update ()
    {

        if (village.isAttacked)
        {
            seuilArmy = village.barbares.nbBarbares;
            int incertitude = Mathf.RoundToInt((seuilArmy/100)*20);
            seuilArmy += Random.Range(-incertitude, incertitude+1);
        }

        seuilNourriture = village.nourrirArmy;
        seuilGold = village.coutNourriture * seuilNourriture;

        alreadyAsk = false;

        if (village.or < seuilGold) NeedGold(seuilGold); 
        else if (village.nourriture < seuilNourriture) NeedFood(seuilNourriture);
        else if (village.army < seuilArmy) NeedArmy(seuilArmy - village.army);
        // else tout va bien alors proposition d'investissement possible
    }

    void Death()
    {
        // DO: this meurt
    }

    void NeedFood(int amount)
    {
        int goldneed = seuilGold*Mathf.RoundToInt(amount/village.coutNourriture);

        if (!alreadyAsk)
        {
            GoAskEmperor(Ressource_Type.food, amount);
            alreadyAsk = true;
        }

        if (village.nourriture < seuilNourriture)
        {
            if (village.or < goldneed) NeedGold(goldneed);
            if (village.or > goldneed) {
                village.or -= goldneed;
                village.nourriture += amount;
            } 
        } else {
            village.or -= goldneed;
            village.nourriture += amount;
        }
    }

    void NeedGold(int amount)
    {
        if (!alreadyAsk)
        {
            GoAskEmperor(Ressource_Type.gold, amount);
            alreadyAsk = true;
        }
    }

    void NeedArmy(int amount)
    {
        int goldneeded = village.costArmy * amount;

        if (!alreadyAsk)
        {
            GoAskEmperor(Ressource_Type.army, amount);
            alreadyAsk = true;
        }

        if (village.or < goldneeded) {
            NeedGold(goldneeded); ;
            if (village.or > goldneeded){
                village.or -= goldneeded;
                village.army += amount;
            }
        } else {
            village.or -= goldneeded;
            village.army += amount;
        }
    }

    void GoAskEmperor(Ressource_Type resource, int amount)
    {
        if (village.or < coutMessager) return;
        village.DecreaseGold(coutMessager);

        switch (resource)
        {
            case Ressource_Type.gold:
                RequestManager.SendRequest(new Request(this,resource,amount));
                return;
            case Ressource_Type.food:
                RequestManager.SendRequest(new Request(this,resource, amount));
                return;
            case Ressource_Type.army:
                RequestManager.SendRequest(new Request(this,resource, amount));
                return;
            default:
                return;
        }
    }

    public void EmperorAsking(Ressource_Type resource, int amount)
    {

    }
}
