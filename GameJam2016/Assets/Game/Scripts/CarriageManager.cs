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
        foreach (Carriage carriage in listCarriage)
        {
            if(carriage.delay <= 0)
            {
                if (!carriage.destination.isDestroyed)
                {
                    if(carriage.amount > 0) { // Give resource to village
                        GiveResources(carriage, carriage.resource, carriage.amount);
                    } else { //Take resource FROM village to capital (instant)
                        int realAmount = carriage.destination.lord.CanYouGive(carriage.resource, carriage.amount);
                        if (realAmount > 0) {
                            TakeResources(carriage, carriage.resource, carriage.amount);
                        }
                    }
                }
                (carriage.provenance as Capitale).nbCharriot++;
                listCarriage.Remove(carriage);
                return;
            }
            carriage.delay--;
        }
    }

    public static void SendCarriage(Carriage carriage)
    {
        carriageManager.listCarriage.Add(carriage);
    }

    public void TakeResources(Carriage carriage, Ressource_Type resource, int amount)
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                carriage.destination.DecreaseGold(amount);
                carriage.provenance.AddGold(amount);
                return;
            case Ressource_Type.food:
                carriage.destination.DecreaseFood(amount);
                carriage.provenance.AddFood(amount);
                return;
            case Ressource_Type.army:
                carriage.destination.DecreaseArmy(amount);
                carriage.provenance.AddArmy(amount);
                return;
            default:
                return;
        }
    }

    public void GiveResources(Carriage carriage, Ressource_Type resource, int amount)
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                carriage.destination.AddGold(amount);
                return;
            case Ressource_Type.food:
                carriage.destination.AddFood(amount);
                return;
            case Ressource_Type.army:
                carriage.destination.AddArmy(amount);
                return;
            default:
                return;
        }
    }

    public int GetCarriageCountAt(Village village)
    {
        int amount = 0;
        foreach(Carriage carriage in listCarriage)
        {
            if (carriage.provenance == village || carriage.destination == village) amount++;
        }
        return amount;
    }
}
