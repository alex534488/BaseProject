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

        seuilNourriture = village.armyFoodCost * village.army;
        seuilGold = village.coutNourriture * seuilNourriture;

        alreadyAsk = false;

        if (village.or < seuilGold) NeedGold(seuilGold); 
        else if (village.nourriture < seuilNourriture) NeedFood(seuilNourriture);
        else if (village.army < seuilArmy) NeedArmy(seuilArmy - village.army);
        if (village.or > seuilGold * 2 && village.or > 10)
        {
            if (Random.Range(0, 101) < village.reputation)
            {
                if (!alreadyAsk)
                {
                    if(cooldown == 0)
                    {
                        RequestManager.SendRequest(new Request(this, Ressource_Type.gold));
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
            GoAskEmperor(Ressource_Type.food, amount);
            alreadyAsk = true;
        }

        if (village.nourriture < seuilNourriture)
        {
            if (village.or < goldneed) NeedGold(goldneed);
            if (village.or > goldneed) {
                village.DecreaseGold(goldneed);
                village.AddFood(amount);
            } else
            {
                village.DecreaseGold(goldneed - village.or);
                village.AddArmy(Mathf.RoundToInt((goldneed - village.or) / village.coutNourriture));
            }
        } else
        {
            village.DecreaseGold(goldneed);
            village.AddFood(amount);
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
                village.DecreaseGold(goldneeded);
                village.AddArmy(amount);
            } else
            {
                village.DecreaseGold(goldneeded - village.or);
                village.AddArmy(Mathf.RoundToInt((goldneeded - village.or)/village.costArmy));
            }
        } else
        {
            village.DecreaseGold(goldneeded);
            village.AddArmy(amount);
        }
    }

    void GoAskEmperor(Ressource_Type resource, int amount)
    {
        if (village.or < coutMessager) return;
        village.DecreaseGold(coutMessager);

        switch (resource)
        {
            case Ressource_Type.gold:
                RequestManager.SendRequest(new Request(this, resource, amount));
                return;
            case Ressource_Type.food:
                RequestManager.SendRequest(new Request(this, resource, amount));
                return;
            case Ressource_Type.army:
                RequestManager.SendRequest(new Request(this,resource, amount));
                return;
            default:
                return;
        }
    }

    public int CanYouGive(Ressource_Type resource) // Modifie
    {
        switch (resource)
        {
            default:
            case Ressource_Type.gold:
                {
                    int value;
                    if ((village.or - seuilGold) < 0)
                    {
                        value = 0;
                    } else
                    {
                        value = (village.or - seuilGold) * (village.reputation / 100);
                    }
                    return value;
                }
             
                
            case Ressource_Type.food:
                {
                    int value;
                    if ((village.nourriture - seuilNourriture) < 0)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = (village.nourriture - seuilNourriture) * (village.reputation / 100);
                    }
                    return value;
                }
                
            case Ressource_Type.army:
                {
                    int value;
                    if ((village.army - seuilArmy) < 0)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = (village.army - seuilArmy) * (village.reputation / 100);
                    }
                    return value;
                }      
        }
    }
}
