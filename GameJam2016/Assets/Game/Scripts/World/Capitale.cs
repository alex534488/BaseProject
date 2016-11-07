using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CCC.Utility;

public class Capitale : Village
{
    class BonheurEvent
    {
        public int at;
        public Request request;
        public BonheurEvent(int at, Request request)
        {
            this.at = at;
            this.request = request;
        }
    }
    List<BonheurEvent> bonheurEvents = new List<BonheurEvent>
    {
        new BonheurEvent(40, EventBonheur1()),
        new BonheurEvent(30, EventBonheur2()),
        new BonheurEvent(20, EventBonheur3()),
        new BonheurEvent(10, EventBonheur4()),
        new BonheurEvent(0, EventBonheur5()),
    };
    List<Request> eventBonheur = new List<Request> { EventBonheur1(), EventBonheur2(), EventBonheur3(), EventBonheur4(), EventBonheur5() };
    int seuilActuel = 0;
    bool bonheurEventTriggered = false;

    // Attribut de la Capitale
    public int startBonusleOr = 10;
    public int startBonusNourriture = 20;
    public int startBonusArmy = 5;

    Stat<int> bonheur;
    int bonheurTop = 10; //Le max de bonheur descend avec le bonheur. 'bonheurTop' est la distance entre le bonheur et le max qui suit

    // Scout
    public int coutScout = 10;

    // Seuil de tolerance permis pour les resources de la capitale
    private int seuilNourritureCapitale;

    // Trade
    public Stat<int> charriot = new Stat<int>(3, 0, 3);
    public int nbTour = 2;
    List<Carriage> listCarriage = new List<Carriage>();

    public Capitale(Empire empire, int id) : base(empire, id, "ROME", null)
    {
        this.id = id;

        bonheur = new Stat<int>(50, 0, 50); //SET LE BONHEUR

        AddGold(startBonusleOr);
        AddFood(startBonusNourriture);
        AddArmy(startBonusArmy);

        lord = new Seigneur(this);
    }

    public override void Update()
    {
        seuilNourritureCapitale = GetArmy();
        bonheurEventTriggered = false;

        UpdateResources();

        CheckResources();

        if (isDestroyed)
        {
            Defaite();
        }
    }

    void Defaite()
    {
        if (isDestroyed)
        {
            DayManager.main.Lose("Les barbares ont envahis votre Empire et détruit votre capitale pour finalement vous découpez en petit morceau afin de faire une délicieuse raclette canibale");
        }
        else
        {
            DayManager.main.Lose("Votre peuple insatisfait vous a trahis en vous poignardant dans le dos");
        }
    }

    #region Resource

    public void AddBonheur(int amount)
    {
        bonheur.Set(bonheur + amount);

        if (amount < 0)
        {
            if ((int)bonheur.MAX > bonheur + bonheurTop)        //Si on diminue le bonheur ET que le max est trop haut
            {
                bonheur.MAX = bonheur + bonheurTop;
            }

            while (bonheurEvents.Count > 0 && bonheurEvents[0].at >= bonheur)  //Bonheur events
            {
                if (bonheurEvents.Count == 1) RequestManager.DeleteAllRequests(); //S'il ne reste que le dernier event -> Defaite

                RequestManager.SendRequest(bonheurEvents[0].request);
                bonheurEvents.RemoveAt(0);
            }
        }
    }

    public int GetBonheur() { return bonheur; }
    
    public int GetBonheurMax() { return (int)bonheur.MAX; }

    public override bool BuyArmy(int amount) //Changement a faire ?
    {
        return base.BuyArmy(amount);
    }

    public override int GetResource(Ressource_Type type)
    {
        if (type == Ressource_Type.bonheur) return GetBonheur();
        else return base.GetResource(type);
    }

    public override int GetResourceAlt(Ressource_Type type)
    {
        if (type == Ressource_Type.bonheur) return GetBonheurMax();
        else return base.GetResourceAlt(type);
    }

    #endregion

    public void SendScout(World theWorld)
    {
        int count = 0;

        AddGold(-Empire.instance.capitale.coutScout);

        foreach (Village village in Empire.instance.listVillage)
        {
            foreach (Barbare barbare in theWorld.barbareManager.listeBarbare)
            {
                if ((barbare.actualTarget == village) && village.barbares.nbBarbares > village.GetArmy())
                {
                    if (!village.lord.alreadyAsk)
                    {
                        RequestManager.SendRequest(new Request(village.lord, Ressource_Type.armé, village.barbares.nbBarbares - village.GetArmy()));
                        count++;
                    }
                }
            }
        }

        // Aucun village n'est attaqué, l'écraireur le signale à la capitale
        List<string> listMessage = new List<string>();
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
        if (charriot <= 0) return;


        if (amount > 0) // Send resources
        {
            if (GetResource(resource) < amount) return; //pas assez de resources
            AddResource(resource, -amount);
            CarriageManager.SendCarriage(new Carriage(nbTour, destination, this, resource, amount));
        }
        else if (amount == -1) //ask for resources
        {
            CarriageManager.SendCarriage(new Carriage(nbTour, destination, this, resource, amount));
        }

        charriot.Set(charriot - 1); //Enleve 1 charriot a la capital
    }

    public override Stat<int>.StatEvent GetStatEvent(Ressource_Type type, bool isAlternative = false)
    {
        Stat<int>.StatEvent ev = base.GetStatEvent(type, isAlternative);
        if (ev == null)
        {
            switch (type)
            {
                case Ressource_Type.bonheur:
                    ev = isAlternative ? bonheur.onMaxSet : bonheur.onSet;
                    break;
            }
        }
        return ev;
    }

    void CheckResources()
    {
        int perteBonheur = 0;
        if (gold < 0) perteBonheur += (Mathf.CeilToInt((-1 * gold) / 5));
        if (food < 0) perteBonheur += (Mathf.CeilToInt((-3 * food) / 5));

        if (perteBonheur > 0) AddBonheur(-perteBonheur);
    }

    static private Request EventBonheur1()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, une certaine agitation commence à parcourir les rue de Rome.\n\nLes gens parlent de vos choix inhumains, et craignent les invasions de barbares.");
        Request request = new Request(listMessage, null);
        return request;
    }

    static private Request EventBonheur2()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, l'agitation grandit au sein de Rome.\n\nCette nuit, elle ont conduit à des affrontements entre vos partisans et d'autres groupes d'individus. \n(-8 Or, -2 Soldat)");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sacrebleu!", delegate () { Empire.instance.capitale.AddGold(-8); Empire.instance.capitale.AddArmy(-2); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur3()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, un groupe contestant votre gouvernance de Rome vient de détruire le Forum.\n\nCela va avoir de sérieuse répercutions sur l'économie de la capitale. (-4 Production Or) ");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Diantre!", delegate () { Empire.instance.capitale.AddGoldProd(-5); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur4()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, de violente émeute éclate en ce moment même dans Rome.\n\nLes répercussions économiques vont être dramatiques. Les réserves de nourritures sont en flammes.");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Malédictition! (-4 Production Nourriture, -20 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-20); Empire.instance.capitale.AddFoodProd(-4); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    static private Request EventBonheur5()
    {
        List<string> listMessage = new List<string>();
        listMessage.Add("Conseiller Brutus : Empereur, le peuple s'est mit d'accord sur votre destitution.\n\nJ'ai peur que votre règne touche à sa fin. ");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Qu'il soit maudit, je resterai leur Empereur jusqu'a ma mort!", delegate () { Empire.instance.capitale.Defaite(); })); // AJOUTER CONDITION FIN

        Request request = new Request(listMessage, listeChoix);
        return request;
    }
}


