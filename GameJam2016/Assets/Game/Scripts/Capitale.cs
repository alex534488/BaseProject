﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Capitale : Village {

    int[] seuilBonheur = {40,30,20,10,0};
    int[] tabBbonheurMax = { 50, 40, 30, 20, 10 };
    Request[] eventBonheur = { EventBonheur1(), EventBonheur2(),EventBonheur3(),EventBonheur4(),EventBonheur5() };
    int seuilActuel = 0;

    // Attribut de la Capitale
    public int capitaleOr = 10;
    public int capitaleNourriture = 20;
    public int capitaleArmy = 5;
    public int bonheur = 50;
    public int bonheurMax;
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

        bonheurMax = tabBbonheurMax[seuilActuel];

        or += capitaleOr;
        nourriture += capitaleNourriture;
        army += capitaleArmy;

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

    public void DecreaseBonheur(int amount)
    {
        bonheur -= amount;
        if(bonheur< seuilBonheur[seuilActuel])
        {
            RequestManager.SendRequest(eventBonheur[seuilActuel]);
            seuilActuel++;
            bonheurMax = tabBbonheurMax[seuilActuel];
        }
    }

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

    static private Request EventBonheur1()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, une certaine agitation commence à parcourir les rue de Rome.");
        listMessage.Add("Les gens parlent de vos choix inhumains, et ont peur de l'avancé des barbares.");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur2()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, l'agitation grandit au sein de Rome.");
        listMessage.Add("Cette nuit, elle ont conduit à des affrontements entre vos partisans et d'autres groupes d'individus. (-8or)");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sacrebleu!", delegate () { Empire.instance.capitale.DecreaseGold(8); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur3()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, un groupe contestant votre gouvernance de Rome vient de détruire le Forum.");
        listMessage.Add("Cela va avoir de sérieuse répercutions sur l'économie de la capitale.(-2 production or");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Diantre!", delegate () { Empire.instance.capitale.productionOr -= 2; }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur4()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, de violente émeute éclate en ce moment même dans Rome.");
        listMessage.Add("Les répercussions économiques vont être dramatiques. Les resserve de nourritures sont en flammes.");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Malédictition! (-2 production nourriture, -6 nourriture)", delegate () { Empire.instance.capitale.DecreaseFood(6);Empire.instance.capitale.productionNourriture -= 2; }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur5()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, le peuple s'est mit d'accord sur votre destitution.");
        listMessage.Add("J'ai peur que votre règne touche à sa fin. ");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Qu'il soit maudit, je resterai leur Empereur jusqu'a ma mort!", delegate () { }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }
}


