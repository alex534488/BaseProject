using UnityEngine;
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
    private int seuilMinimalArmy = 3;
    public int seuilArmy = 0;

    // Investissement
    private int cooldown = 0;


    // Es ce que le seigneur a deja demander a l'emperor
    public bool alreadyAsk = false;

    public Seigneur(Village village)
    {
        this.village = village;
        seuilNourriture = village.armyFoodCost;
        seuilGold = village.coutNourriture * village.armyFoodCost;
        seuilArmy = 0;
        cooldown += village.id * 2;
    }
	
	public void Update ()
    {
        if (village.isAttacked)
        {
            seuilArmy = village.barbares.nbBarbares;
            int incertitude = Mathf.RoundToInt((seuilArmy/100)*20);
            seuilArmy += Random.Range(-incertitude, incertitude+1);
        }

        seuilNourriture = village.armyFoodCost * village.GetArmy();
        seuilGold = village.coutNourriture * seuilNourriture;

        alreadyAsk = false;

        if (village.GetGold() < seuilGold) NeedGold(seuilGold); 
        else if (village.GetFood() < seuilNourriture) NeedFood(seuilNourriture);
        else if (village.GetArmy() < seuilArmy) NeedArmy(seuilArmy - village.GetArmy());
        if (village.GetGold() > seuilGold * 2 && village.GetGold() > 10)
        {
            if (Random.Range(0, 101) < village.GetReputation())
            {
                if (!alreadyAsk)
                {
                    if(cooldown == 0)
                    {
                        RequestManager.SendRequest(new Request(this, Resource_Type.gold));
                        alreadyAsk = true;
                        cooldown = 3;
                    }    
                }
            }
        }
        if (cooldown == 0)
        {
            cooldown = 3; 
        } else cooldown--;
    }

    public void Death()
    {
        RequestManager.SendRequest(new Request(this));
    }

    void NeedFood(int amount)
    {
        int goldneed = seuilGold*Mathf.RoundToInt(amount/village.coutNourriture);

        if (!alreadyAsk)
        {
            GoAskEmperor(Resource_Type.food, amount);
            alreadyAsk = true;
        }

        if (village.GetFood() < seuilNourriture)
        {
            if (village.GetGold() < goldneed) NeedGold(goldneed);
            if (village.GetGold() > goldneed) {
                village.AddGold(-goldneed);
                village.AddFood(amount);
            } else
            {
                village.AddGold(-(goldneed - village.GetGold()));
                village.AddArmy(Mathf.RoundToInt((goldneed - village.GetGold()) / village.coutNourriture));
            }
        } else
        {
            village.AddGold(-goldneed);
            village.AddFood(amount);
        }
    }

    void NeedGold(int amount)
    {
        if (!alreadyAsk)
        {
            GoAskEmperor(Resource_Type.gold, amount);
            alreadyAsk = true;
        }
    }

    void NeedArmy(int amount)
    {
        int goldneeded = village.costArmy * amount;

        if (!alreadyAsk)
        {
            GoAskEmperor(Resource_Type.army, amount);
            alreadyAsk = true;
        }

        if (village.GetGold() < goldneeded) {
            NeedGold(goldneeded); ;
            if (village.GetGold() > goldneeded){
                village.AddGold(-goldneeded);
                village.AddArmy(amount);
            } else
            {
                village.AddGold(-(goldneeded - village.GetGold()));
                village.AddArmy(Mathf.RoundToInt((goldneeded - village.GetGold())/village.costArmy));
            }
        } else
        {
            village.AddGold(-goldneeded);
            village.AddArmy(amount);
        }
    }

    void GoAskEmperor(Resource_Type resource, int amount)
    {
        if (village.GetGold() < coutMessager) return;
        village.AddGold(-coutMessager);

        switch (resource)
        {
            case Resource_Type.gold:
                RequestManager.SendRequest(new Request(this, resource, amount));
                return;
            case Resource_Type.food:
                RequestManager.SendRequest(new Request(this, resource, amount));
                return;
            case Resource_Type.army:
                RequestManager.SendRequest(new Request(this,resource, amount));
                return;
            default:
                return;
        }
    }

    public int CanYouGive(Resource_Type resource) // Modifie
    {
        switch (resource)
        {
            default:
            case Resource_Type.gold:
                {
                    int value;
                    if ((village.GetGold() - seuilGold) < 0)
                    {
                        value = 0;
                    } else
                    {
                        value = (village.GetGold() - seuilGold) * (village.GetReputation() / 100);
                    }
                    return value;
                }
             
                
            case Resource_Type.food:
                {
                    int value;
                    if ((village.GetFood() - seuilNourriture) < 0)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = (village.GetFood() - seuilNourriture) * (village.GetReputation() / 100);
                    }
                    return value;
                }
                
            case Resource_Type.army:
                {
                    int value;
                    if ((village.GetArmy() - seuilArmy) < 0)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = (village.GetArmy() - seuilArmy) * (village.GetReputation() / 100);
                    }
                    return value;
                }      
        }
    }
}
