using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class CartsManager : INewDay
{
    List<Cart> listCarriage = new List<Cart>();

    private Stat<int> availableCarts = new Stat<int>(0, 0, 10, Stat<int>.BoundMode.Cap);

    public int AvailableCarts
    {
        get { return availableCarts; }
    }

    public Stat<int>.StatEvent OnAvailableCartChange
    {
        get { return availableCarts.onSet; }
    }

    //TODO: Ce qu'il y a dans cette fonction marchait, mais c'était un peut difficile de s'y retrouver.
    //      Je propose qu'on tente de la subdivisé en plus petite tache/fonction si possible
    public void NewDay()
    {
        /*
        // Compteur de tour
        for (int i = 0; i < listCarriage.Count; i++)
        {
            Cart carriage = listCarriage[i];
            if (carriage.delay <= 0)
            {
                //print("arrivee");
                if (!carriage.destination.isDestroyed)
                {
                    if (carriage.amount > 0)                                   // Give resource to village
                    {
                        carriage.destination.AddResource(carriage.resource, carriage.amount);
                        carriage.destination.AddReputation(10);
                        // NB. les resources sont deja enlevees dans le script capitale donc on ne fait que donner
                    }
                    else                                                       //Take resource FROM village to capital (instant)
                    {
                        if (carriage.amount != 0)
                        {
                            RequestManager.BuildAndSendRequest("carriage_return", carriage.destination, carriage.provenance, -1 * carriage.amount, carriage.resource);
                        } else RequestManager.SendRequest(new Request(carriage, 0));
                    }
                    Empire.instance.capitale.charriot.Set(Empire.instance.capitale.charriot + 1);
                }
                else
                {
                    RequestManager.SendRequest(new Request(carriage, -1));
                }
                this.listCarriage.RemoveAt(i);
                i--;
            }
            else
            {
                carriage.delay--;
            }
        }
        */
    }

    //public static void SendCarriage(Cart carriage)
    //{
    //    cartsManager.listCarriage.Add(carriage);

    //    // Si le chariot est une requete de resources a un village
    //    if (carriage.destination.GetType() == typeof(Capitale)) { carriage.amount = -1 * carriage.destination.lord.CanYouGive(carriage.resource); } // calcul le montant sans en faire l'application
    //}

    //public static int GetCarriageCountAt(Village village)
    //{
    //    int amount = 0;
    //    foreach (Cart carriage in cartsManager.listCarriage)
    //    {
    //        if (carriage.provenance == village || carriage.destination == village) amount++;
    //    }
    //    return amount;
    //}
}
