using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;


[System.Serializable]
public class Cart
{
    private int delayCounter;
    private bool sent = false;
    private bool arrived = false;

    public bool IsArrived
    {
        get { return arrived; }
    }
    public bool Sent
    {
        get { return sent; }
    }
    public int RemainingDays
    {
        get { return delayCounter; }
    }
    public int MapSource
    {
        get { return mapSource; }
    }
    public int MapDestination
    {
        get { return mapDestination; }
    }
    public ReadOnlyCollection<Transaction> StartTransactions
    {
        get { return startTransactions != null ? startTransactions.AsReadOnly() : null; }
    }
    public ReadOnlyCollection<Transaction> ArriveTransactions
    {
        get { return arriveTransactions != null ? arriveTransactions.AsReadOnly() : null; }
    }

    private List<Transaction> startTransactions;
    private List<Transaction> arriveTransactions;

    //Map position
    private int mapSource;
    private int mapDestination;

    public Cart(int delay, Village visualSource, Village visualDestination, List<Transaction> transactions = null) :
        this(delay, visualDestination.GetMapPosition(), visualSource.GetMapPosition(), transactions)
    { }

    public Cart(int delay, int mapSource, int mapDestination, List<Transaction> transactions = null)
    {
        delayCounter = delay;
        this.mapDestination = mapDestination;
        this.mapSource = mapSource;
        if (transactions != null)
            foreach (Transaction transaction in transactions)
                AddTransaction(transaction);
    }

    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {

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
        if (startTransactions != null)
            foreach (Transaction transaction in startTransactions)
            {
                transaction.Execute();
            }
    }

    /// <summary>
    /// Retourne true si le cart est arrivé à sa destination
    /// </summary>
    public void Progress()
    {
        delayCounter--;

        if (delayCounter <= 0 && !arrived)
        {
            Arrive();
        }
    }

    public void Arrive()
    {
        if (arrived)
            return;

        arrived = true;

        if (arriveTransactions != null)
            if (VillageStillExists(mapDestination))
            {
                foreach (Transaction transaction in arriveTransactions)
                    transaction.Execute();
            } else
            {
                CancelCart();
            }
    }

    private bool VillageStillExists(int pos)
    {
        Village village = Universe.Map.GetVillage(pos);
        return village == null ? false : village.IsDestroyed;
    }

    private void CancelCart()
    {
        // Le cart redonne les ressources à sa source
        mapDestination = mapSource;

        if (!VillageStillExists(mapDestination))
            return;

        Village dest = Universe.Map.GetVillage(mapDestination);

        foreach (Transaction transaction in arriveTransactions)
        {
            transaction.destination = dest;
            transaction.Execute();
        }
    }
}
