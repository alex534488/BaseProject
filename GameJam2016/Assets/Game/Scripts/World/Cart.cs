using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Cart {

    public static int stdDelay = 2;
    public int delay;
    public Village provenance;
    public Village destination;
    public Resource_Type resource;
    public int amount;

	public Cart(int delay, Village destination, Village provenance, Resource_Type resource, int amount)
    {
        this.provenance = provenance;
        this.delay = delay;
        this.destination = destination;
        this.resource = resource;
        this.amount = amount;
    }

    public void CompletedTradeGift()
    {
        switch (resource)
        {
            case Resource_Type.gold:
                destination.AddGold(amount);
                return;
            case Resource_Type.food:
                destination.AddFood(amount);
                return;
            case Resource_Type.army:
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
            case Resource_Type.gold:
                provenance.AddGold(amount);
                return;
            case Resource_Type.food:
                provenance.AddFood(amount);
                return;
            case Resource_Type.army:
                provenance.AddArmy(amount);
                return;
            default:
                return;
        }
    }
}
