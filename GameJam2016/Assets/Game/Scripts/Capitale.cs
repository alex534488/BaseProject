using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Capitale : Village {

    // Attribut de la Capitale
    public int capitaleOr = 10;
    public int capitaleNourriture = 20;
    public int capitaleArmy = 5;
    public int bonheur = 100;

    // Scout
    public int coutScout = 10;

    // Trade
    public int nbCharriot = 3;
    public int nbTour = 2;
    List<Carriage> listCarriage = new List<Carriage>();

    public Capitale(Empire empire, int id) : base(empire,id, "ROME", null)
    {
        this.empire = empire;
        this.id = id;

        or = capitaleOr;
        nourriture = capitaleNourriture;
        army = capitaleArmy;

        lord = new Seigneur(this);
    }
	
	public override void Update ()
    {


        if (nourriture < 0 || bonheur < 0 || isDestroyed)
        {
            DestructionVillage();
        }
        nourrirArmy = army;

        UpdateResources();

        UpdateCost();

    }

    public void DecreaseBonheur(int amount) { bonheur -= amount; }

    public void AddBonheur(int amount) { bonheur += amount; }

    public void DecreaseChariot(int amount) { nbCharriot -= amount; }

    public void AddChariot(int amount) { nbCharriot += amount; }

    public void SendScout(World theWorld)
    {
        DecreaseGold(empire.capitale.coutScout);

        foreach (Village village in empire.listVillage)
        {
            foreach(Barbare barbare in theWorld.listBarbare)
            {
                if ((barbare.actualTarget == village) && village.barbares.nbBarbares > village.army)
                {
                    if (!village.lord.alreadyAsk)
                    {
                        RequestManager.SendRequest(new Request(village.lord, Ressource_Type.army, village.barbares.nbBarbares - village.army));
                    }
                }
            }
           
        }

        // Le scout revient dans X tours?
    }

    public void SendCartToVillage(Village destination, Ressource_Type resource, int amount)
    {
         CarriageManager.SendCarriage(new Carriage(nbTour, destination,resource,amount));
    }
}
