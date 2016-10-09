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
        for(int i = 0; i < listCarriage.Count; i++)
        {
            if(listCarriage[i].delay <= 0)
            {
                if (!listCarriage[i].destination.isDestroyed)
                {
                    if(listCarriage[i].amount > 0) { // Give resource to village
                        GiveResources(listCarriage[i], listCarriage[i].resource, listCarriage[i].amount);
                        listCarriage[i].destination.AddReputation(10);
                        (listCarriage[i].provenance as Capitale).AddChariot(1);
                    } 
                        else { //Take resource FROM village to capital (instant)
                        int realAmount = listCarriage[i].destination.lord.CanYouGive(listCarriage[i].resource);
                        if (realAmount > 0) {
                            TakeResources(listCarriage[i], listCarriage[i].resource, realAmount);
                            listCarriage[i].destination.DecreaseReputation(10);
                            Empire.instance.capitale.AddChariot(1);
                            RequestManager.SendRequest(new Request(listCarriage[i], realAmount));
                        }
                    }
                }
                this.listCarriage.Remove(listCarriage[i]);
            }
            listCarriage[i].delay--;
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

    public static int GetCarriageCountAt(Village village)
    {
        int amount = 0;
        foreach(Carriage carriage in carriageManager.listCarriage)
        {
            if (carriage.provenance == village || carriage.destination == village) amount++;
        }
        return amount;
    }
}
