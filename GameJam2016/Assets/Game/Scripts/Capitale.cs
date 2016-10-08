﻿using UnityEngine;
using System.Collections;

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
    public int nbTour;

    // Bonheur
    public int productionBonheur = 1;

    public Capitale(Empire empire, int id) : base(empire,id, "ROME", null)
    {
        this.empire = empire;
        this.id = id;

        or = capitaleOr;
        nourriture = capitaleNourriture;
        army = capitaleArmy;

        lord = new Seigneur(this);
    }
	
	public override void Update () // es ce que le override rajoute ou remplace?
    {
        base.Update();

        if(productionBonheur > 0) AddBonheur(productionBonheur * random);
        if (productionBonheur < 0) DecreaseBonheur(productionBonheur * random);
    }

    void DecreaseBonheur(int amount) { bonheur -= amount; }

    void AddBonheur(int amount) { bonheur += amount; }

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

    void RequestVillage(Village village, Ressource_Type resource, int amount)
    {
        nbCharriot--;

        switch (resource)
        {
            case Ressource_Type.gold:
                DecreaseGold(amount);
                return;
            case Ressource_Type.food:
                DecreaseFood(amount);
                return;
            case Ressource_Type.army:
                DecreaseArmy(amount);
                return;
            default:
                return;
        }
    }

    void GiveVillage(Village village, Ressource_Type resource, int amount)
    {
        nbCharriot--;

    }
}
