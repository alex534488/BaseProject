using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Capitale : Village
{

    int[] seuilBonheur = { 40, 30, 20, 10, 0 };
    int[] tabBonheurMax = { 50, 40, 30, 20, 10 };
    Request[] eventBonheur = { EventBonheur1(), EventBonheur2(), EventBonheur3(), EventBonheur4(), EventBonheur5() };
    int seuilActuel = 0;

    // Attribut de la Capitale
    public int capitaleOr = 10;
    public int capitaleNourriture = 20;
    public int capitaleArmy = 5;
    public int bonheur = 50;
    public int bonheurMax;

    // Scout
    public int coutScout = 10;

    // Seuil de tolerance permis pour les resources de la capitale
    private int seuilNourritureCapitale;

    // Trade
    public int nbCharriot = 3;
    public int nbTour = 2;
    List<Carriage> listCarriage = new List<Carriage>();

    //Events
    public StatEvent onBonheurChange = new StatEvent();
    public StatEvent onBonheurMaxChange = new StatEvent();

    public Capitale(Empire empire, int id) : base(empire, id, "ROME", null)
    {
        this.empire = empire;
        this.id = id;

        SetBonheurMax(tabBonheurMax[seuilActuel]);

        AddGold(capitaleOr);
        AddFood(capitaleNourriture);
        AddArmy(capitaleArmy);

        lord = new Seigneur(this);
    }

    public override void Update()
    {
        seuilNourritureCapitale = army;

        if (nourriture < 0 || bonheur < 0 || isDestroyed)
        {
            DestructionVillage();
        }

        UpdateResources();

        UpdateCost();

        CheckResources();
    }

    public void DecreaseBonheur(int amount)
    {
        bonheur -= amount;
        if (bonheur < seuilBonheur[seuilActuel])
        {
            RequestManager.SendRequest(eventBonheur[seuilActuel]);
            seuilActuel++;
            SetBonheurMax(tabBonheurMax[seuilActuel]);
        }
        onBonheurChange.Invoke(-amount);
    }

    public void AddBonheur(int amount)
    {
        amount = Mathf.Min(bonheurMax - bonheur, amount);
        bonheur += amount; onBonheurChange.Invoke(amount);
    }

    public void SetBonheurMax(int amount) { bonheurMax = amount; onBonheurMaxChange.Invoke(amount); }

    public void DecreaseChariot(int amount) { nbCharriot -= amount; }

    public void AddChariot(int amount) { nbCharriot += amount; }

    public void SendScout(World theWorld)
    {
        int count = 0;

        DecreaseGold(empire.capitale.coutScout);

        foreach (Village village in empire.listVillage)
        {
            foreach (Barbare barbare in theWorld.barbareManager.listeBarbare)
            {
                if ((barbare.actualTarget == village) && village.barbares.nbBarbares > village.army)
                {
                    if (!village.lord.alreadyAsk)
                    {
                        RequestManager.SendRequest(new Request(village.lord, Ressource_Type.army, village.barbares.nbBarbares - village.army));
                        count++;
                    }
                }
            }
        }

        // Aucun village n'est attaqué, l'écraireur le signale à la capitale
        List<string >listMessage = new List<string>();
        listMessage.Add(" Salutation, je suis votre humble éclaireur qui est de retour de voyage." + "\n \n" + " L'état de la situation actuelle n'est vraiment pas allarmante. Les barbares semblent être tranquille et n'attaque pas les villages.");
        listMessage.Add(" Chacun d'entre eux se porte bien et votre assistance n'est pas necessaire pour le moment" + "\n \n" + " Nous sommes dans une période de paix. N'hésitez à me renvoyer faire le tour des villages pour que je vous signale la situation des villages");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix(" Merci éclaireur! Bonne journée à toi mon ami.", delegate () { }));
        Request request = new Request(listMessage, listeChoix);

        if (count <= 0) RequestManager.SendRequest(request);
        count = 0;
    }

    public void SendCartToVillage(Village destination, Ressource_Type resource, int amount)
    {
        if (nbCharriot <= 0) return;


        if (amount > 0 && GetTotal(resource) > amount )
        {
            ModifyResource(resource, -amount);
            CarriageManager.SendCarriage(new Carriage(nbTour, destination, this, resource, amount));
        }
        else if (amount == -1)
        {
            CarriageManager.SendCarriage(new Carriage(nbTour,this , destination, resource, amount));
        }
        
        nbCharriot --;
    }

    public override StatEvent GetStatEvent(Ressource_Type type, bool isAlternative = false)
    {
        StatEvent ev = base.GetStatEvent(type, isAlternative);
        if (ev == null)
        {
            switch (type)
            {
                case Ressource_Type.happiness:
                    ev = isAlternative ? onBonheurMaxChange : onBonheurChange;
                    break;
            }
        }
        return ev;
    }

    void CheckResources()
    {
        if (nourriture < seuilNourritureCapitale) BesoinNourriture(seuilNourritureCapitale - nourriture);
        if (bonheur <= 0) Defaite("Trahison");

        int perteBonheur = 0;
        if (or < 0) perteBonheur += (Mathf.CeilToInt((-1 * or) / 5));
        if (nourriture < 0) perteBonheur += (Mathf.CeilToInt((-3 * nourriture) / 5));

        if (perteBonheur > 0)
        {
            if (perteBonheur >= bonheur)
                DecreaseBonheur(bonheur);

            else
                DecreaseBonheur(perteBonheur);
        }
    }

    void BesoinNourriture(int amount)
    {
        int goldNeeded = coutNourriture * amount;

        if (or < goldNeeded) return;

        ModifyResource(Ressource_Type.gold, amount);
        ModifyResource(Ressource_Type.food, amount);
    }

    void Defaite(string type)
    {
        // Fin de la partie
        switch (type)
        {
            case "Trahison":
                break;
            case "Famine":
                break;
            case "Militaire":
            default:
                break;
        }
    }

    static private Request EventBonheur1()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, une certaine agitation commence à parcourir les rue de Rome.\n\nLes gens parlent de vos choix inhumains, et craignent les invasions de barbares.");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur2()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, l'agitation grandit au sein de Rome.\n\n Cette nuit, elle ont conduit à des affrontements entre vos partisans et d'autres groupes d'individus. (-8 Or, -2 Soldat)");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sacrebleu!", delegate () { Empire.instance.capitale.DecreaseGold(8); Empire.instance.capitale.DecreaseArmy(2); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur3()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, un groupe contestant votre gouvernance de Rome vient de détruire le Forum.\n\nCela va avoir de sérieuse répercutions sur l'économie de la capitale. (-4 Production Or) ");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Diantre!", delegate () { Empire.instance.capitale.ModifyGoldProd(-5); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur4()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, de violente émeute éclate en ce moment même dans Rome.\n\nLes répercussions économiques vont être dramatiques. Les réserves de nourritures sont en flammes.");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Malédictition! (-4 Production Nourriture, -20 Nourriture)", delegate () { Empire.instance.capitale.DecreaseFood(20);Empire.instance.capitale.ModifyFoodProd(-4); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur5()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, le peuple s'est mit d'accord sur votre destitution.\n\nJ'ai peur que votre règne touche à sa fin. ");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Qu'il soit maudit, je resterai leur Empereur jusqu'a ma mort!", delegate () { })); // AJOUTER CONDITION FIN
        Request request = new Request(listMessage, listeChoix);
        return request;
    }
}


