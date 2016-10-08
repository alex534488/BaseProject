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
        base.Update();
    }

    void DecreaseBonheur(int amount) { bonheur -= amount; }

    void AddBonheur(int amount) { bonheur += amount; }

    void DecreaseChariot(int amount) { nbCharriot -= amount; }

    void AddChariot(int amount) { nbCharriot += amount; }

    public void SendScout()
    {
        DecreaseGold(empire.capitale.coutScout);
        /* Le systeme de bataille et de barbares n'interagit pas assez avec le world pour pouvoir faire un scout fonctionnel
        foreach (Village village in empire.listVillage)
        {
            if (village.isAttacked && village.barbares.nbBarbares > village.army)
            {
                if (!village.lord.alreadyAsk)
                {
                    RequestManager.SendRequest(new Request(village.lord, Ressource_Type.army, village.barbares.nbBarbares - village.army));
                }
            }
        }
        */
        // Le scout revient dans X tours?
    }

    public void SendCartToVillage(Village destination, Ressource_Type resource, int amount)
    {
         CarriageManager.SendCarriage(new Carriage(nbTour, destination,resource,amount));
    }
}
