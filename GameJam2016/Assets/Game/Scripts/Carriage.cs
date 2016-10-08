using UnityEngine;
using System.Collections;

public class Carriage {

    public int delay;
    public Village destination;
    public Ressource_Type resource;
    public int amount;
    public bool idle = true;

	public Carriage(int delay, Village destination, Ressource_Type resource, int amount)
    {
        this.delay = delay;
        this.destination = destination;
        this.resource = resource;
        this.amount = amount;
    }
}
