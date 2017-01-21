using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum DestinationType
{
    village = 1,
    tradeRoutes = 2,
    explore = 3
}

public class Cart
{

    private int delay;
    private int delayCounter;
    private DestinationType type;
    private bool sent = false;
    private bool arrived = false;

    public bool Arrived
    {
        get { return arrived; }
    }
    public bool Sent
    {
        get { return sent; }
    }

    /*
    private List<Village_ResourceType> resourceVillage;
    private List<Empire_ResourceType> resourceEmpire;
    private List<int> resourceVillageAmount;
    private List<int> resourceEmpireAmount;*/

    private List<Transaction> transactions;

    private List<Transaction> startTransactions;
    private List<Transaction> arriveTransactions;

    //Map position
    private int visualSource;
    private int visualDestination;

    public Cart(int delay, Village visualSource, Village visualDestination, List<Transaction> transactions = null) :
        this(delay, visualDestination.GetMapPosition(), visualSource.GetMapPosition(), transactions)
    { }

    public Cart(int delay, int visualSource, int visualDestination, List<Transaction> transactions = null)
    {
        this.delay = delay;
        this.visualDestination = visualDestination;
        this.visualSource = visualSource;
        foreach (Transaction transaction in transactions)
            AddTransaction(transaction);
    }

    public void AddTransaction(Transaction transaction)
    {
        Transaction start;
        Transaction arrive;

        transaction.Split(out start, out arrive);

        AddStartTransaction(start);
        AddArriveTransaction(arrive);
    }

    public void AddStartTransaction(Transaction transaction)
    {
        if (startTransactions == null)
            startTransactions = new List<Transaction>();
        startTransactions.Add(transaction);
    }

    public void AddArriveTransaction(Transaction transaction)
    {
        if (arriveTransactions == null)
            arriveTransactions = new List<Transaction>();
        arriveTransactions.Add(transaction);
    }

    //Appelé lorsque le cart est belle et bien envoyé (par le cart manager)
    // C'est ici qu'on devrais faire payer la source
    public void Send()
    {
        if (sent)
            return;

        sent = true;

        //On execute tous les transaction de départ
        foreach (Transaction transaction in startTransactions)
        {
            transaction.Execute();
        }
    }

    /// <summary>
    /// Retourne true si le cart est arrivé à sa destination
    /// </summary>
    public bool Progress()
    {
        delayCounter--;
        if (delayCounter <= 0)
        {
            Arrive();
            return true;
        }
        return false;
    }

    public void Arrive()
    {
        if (arrived)
            return;

        arrived = true;

        switch (type)
        {
            default:
                return;
            case DestinationType.village:
                foreach (Transaction transaction in arriveTransactions)
                    transaction.Execute();
                return;
            case DestinationType.tradeRoutes:
                // Donne un montant d'or x
                return;
            case DestinationType.explore:
                // Donne des resources d'empire aleatoire
                return;
        }
    }
}
