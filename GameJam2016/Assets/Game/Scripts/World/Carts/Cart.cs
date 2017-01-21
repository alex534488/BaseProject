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

public class Cart {

    private int delay;
    private int delayCounter;

    private Village provenance;
    private Village villageDestination;
    private DestinationType destination;

    private List<Village_ResourceType> resourceVillage;
    private List<Empire_ResourceType> resourceEmpire;
    private List<int> resourceVillageAmount;
    private List<int> resourceEmpireAmount;

	public Cart(int delay, Village provenance, DestinationType destination, List<int> resourceVillageAmount, List<int> resourceEmpireAmount, Village villageDestination = null, List<Village_ResourceType> resourceVillage = null, List<Empire_ResourceType> resourceEmpire = null)
    {
        this.delay = delay;

        this.resourceVillage = resourceVillage;
        this.resourceEmpire = resourceEmpire;

        if(resourceVillage.Count < resourceVillageAmount.Count || resourceVillageAmount.Count < resourceVillage.Count)
        {
            // Erreur!
        }

        if (resourceEmpire.Count < resourceEmpireAmount.Count || resourceEmpireAmount.Count < resourceEmpire.Count)
        {
            // Erreur!
        }

        this.resourceVillageAmount = resourceVillageAmount;
        this.resourceEmpireAmount = resourceEmpireAmount;

        this.provenance = provenance;
        this.destination = destination;
        this.villageDestination = villageDestination;
    }

    public bool Update()
    {
        delayCounter--;
        if(delayCounter <= 0)
        {
            return true;
        }
        return false;
    }

    public void Apply()
    {
        switch (destination)
        {
            default:
                return;
            case DestinationType.village:
                for(int i = 0; i < resourceVillage.Count; i++)
                {
                    villageDestination.Add(resourceVillage[i],resourceVillageAmount[i]);
                }
                for (int i = 0; i < resourceEmpire.Count; i++)
                {
                    Universe.Empire.Add(resourceEmpire[i], resourceEmpireAmount[i]);
                }
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
