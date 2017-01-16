using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Cart {

    public static int stdDelay = 2;
    public int delay;
    public Village provenance;
    public Village destination;
    public Village_ResourceType resource;
    public int amount;

	public Cart(int delay, Village destination, Village provenance, Village_ResourceType resource, int amount)
    {
        this.provenance = provenance;
        this.delay = delay;
        this.destination = destination;
        this.resource = resource;
        this.amount = amount;
    }
}
