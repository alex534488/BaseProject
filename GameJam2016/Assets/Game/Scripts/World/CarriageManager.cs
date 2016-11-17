using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CarriageManager : MonoBehaviour
{

    static CarriageManager carriageManager;

    List<Carriage> listCarriage = new List<Carriage>();

    public UnityEvent OnArriveDestination = new UnityEvent();

    void Awake()
    {
        if (carriageManager == null) carriageManager = this;
    }

    public void NewDay()
    {
        // Compteur de tour
        for (int i = 0; i < listCarriage.Count; i++)
        {
            Carriage carriage = listCarriage[i];
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
    }

    public static void SendCarriage(Carriage carriage)
    {
        carriageManager.listCarriage.Add(carriage);

        // Si le chariot est une requete de resources a un village
        if (carriage.destination.GetType() == typeof(Capitale)) { carriage.amount = -1 * carriage.destination.lord.CanYouGive(carriage.resource); } // calcul le montant sans en faire l'application
    }

    public static int GetCarriageCountAt(Village village)
    {
        int amount = 0;
        foreach (Carriage carriage in carriageManager.listCarriage)
        {
            if (carriage.provenance == village || carriage.destination == village) amount++;
        }
        return amount;
    }
}
