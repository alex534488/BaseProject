using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Cart {

    public static int stdDelay = 2;
    public int delay;
    public Village provenance;
    public Village destination;
    public ResourceType resource;
    public int amount;

	public Cart(int delay, Village destination, Village provenance, ResourceType resource, int amount)
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
            case ResourceType.gold:
                destination.AddGold(amount);
                return;
            case ResourceType.food:
                destination.AddFood(amount);
                return;
            case ResourceType.army:
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
            case ResourceType.gold:
                provenance.AddGold(amount);
                return;
            case ResourceType.food:
                provenance.AddFood(amount);
                return;
            case ResourceType.army:
                provenance.AddArmy(amount);
                return;
            default:
                return;
        }
    }
}
