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
                    if(carriage.amount < 0) {
                        GiveResources(carriage, carriage.resource, carriage.amount);
                        listCarriage.Remove(carriage);
                    } else {
                        if (carriage.destination.lord.CanYouGive(carriage.resource, carriage.amount)) {
                            TakeResources(carriage, carriage.resource, carriage.amount);
                            listCarriage.Remove(carriage);
                        }
                    }

                }
                carriage.delay--;
            }
        }
    }

    public static void SendCarriage(Carriage carriage)
    {
        carriageManager.listCarriage.Add(carriage);
    }

    public void TakeResources(Carriage carriage, Ressource_Type resource, int amount)
    {
        int newamount = (-1 * amount) / carriage.destination.reputation;

        switch (resource)
        {
            case Ressource_Type.gold:
                carriage.destination.DecreaseGold(newamount);
                carriage.provenance.AddGold(newamount);
                return;
            case Ressource_Type.food:
                carriage.destination.DecreaseFood(newamount);
                carriage.provenance.AddFood(newamount);
                return;
            case Ressource_Type.army:
                carriage.destination.DecreaseArmy(newamount);
                carriage.provenance.AddArmy(newamount);
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
}
