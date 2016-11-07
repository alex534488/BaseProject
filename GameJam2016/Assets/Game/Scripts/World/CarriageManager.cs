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
        for (int i = 0; i < listCarriage.Count; i++)
        {
            Carriage carriage = listCarriage[i];
            if (carriage.delay <= 0)
            {
                print("arrivee");
                if (!carriage.destination.isDestroyed)
                {
                    if (carriage.amount > 0)                                   // Give resource to village
                    {
                        carriage.destination.AddResource(carriage.resource, carriage.amount);
                        carriage.destination.AddReputation(10);
                    }
                    else                                                       //Take resource FROM village to capital (instant)
                    {
                        int realAmount = carriage.destination.lord.CanYouGive(carriage.resource);
                        if (realAmount > 0)
                        {
                            Village.Transfer(carriage.destination, carriage.provenance, carriage.resource, realAmount);
                            carriage.destination.AddReputation(-10);
                            RequestManager.SendRequest(new Request(carriage, realAmount));
                        }
                        else RequestManager.SendRequest(new Request(carriage, 0));
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
