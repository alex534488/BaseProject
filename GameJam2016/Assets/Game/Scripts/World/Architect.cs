using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Architect : INewDay
{
    private List<Building> buildings = new List<Building>(); // Liste de buildings construit dans le village

    public void NewDay()
    {
        // TODO : Verification Quotidienne
    }

    public Building GetBuildingFromBank(string name, Village village)
    {
        return BuildingBank.GetBuilding(name, village);
    }

    public Building GetBuilding(string name)
    {
        for(int i = 0; i < buildings.Count; i++)
        {
            if(buildings[i].GetName() == name)
            {
                return buildings[i];
            }
        }
        return null;
    }

    // Construire un batiment
    public void Build(string name, Village village)
    {
        Building newBuilding = GetBuildingFromBank(name, village); // Trouver le building
        buildings.Add(newBuilding); // L'ajouter a la liste de building construit
        // S'il a un building behavior
        if (newBuilding.GetComponent<BuildingBehavior>() != null)
        {
            // Si on effectue seulement la fonction du building behavior
            if (newBuilding.buildingBehavior.OnBuy()) return;
        }
        newBuilding.Apply(village); // Appliquer la construction du building
    }
}
