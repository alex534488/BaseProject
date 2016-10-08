using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Carriage {

    public int delay;
    public Village provenance;
    public Village destination;
    public Ressource_Type resource;
    public int amount;

	public Carriage(int delay, Village destination, Ressource_Type resource, int amount)
    {
        this.delay = delay;
        this.destination = destination;
        this.resource = resource;
        this.amount = amount;
    }

    public void CompletedTradeGift()
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                destination.AddGold(amount);
                return;
            case Ressource_Type.food:
                destination.AddFood(amount);
                return;
            case Ressource_Type.army:
                destination.AddArmy(amount);
                return;
            default:
                return;
        }
    }

    public void CompletedTradeRequest()
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                provenance.AddGold(amount);
                return;
            case Ressource_Type.food:
                provenance.AddFood(amount);
                return;
            case Ressource_Type.army:
                provenance.AddArmy(amount);
                return;
            default:
                return;
        }
    }
}
