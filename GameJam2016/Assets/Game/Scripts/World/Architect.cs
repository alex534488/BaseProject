using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Architect : INewDay
{
    Village myVillage;

    public Architect(Village village)
    {
        myVillage = village;
    }

    [System.NonSerialized]
    private List<Building> buildings = new List<Building>(); // Liste de buildings construit dans le village
    private List<string> buildingsName = new List<string>(); // Liste des NOMS de buildings construit dans le village

    // Dictionnaire contenant tout les behavior particuliere de building
    private Dictionary<string, BuildingBehavior> behaviors = new Dictionary<string, BuildingBehavior>();

    //Appelé lorsqu'on load une partie 
    public void OnLoad()
    {
        buildings = new List<Building>();
        foreach (string name in buildingsName)
            Rebuild(name);
    }

    public void NewDay()
    {
        // TODO : Verification Quotidienne
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

    /// <summary>
    /// Regénère les liens avec les buildings (seulement appelé lors du OnLoad())
    /// </summary>
    private void Rebuild(string name)
    {
        Building newBuilding = BuildingBank.CreateBuilding(name); // Trouver le building
        buildings.Add(newBuilding); // L'ajouter a la liste de building construit
    }

    // Construire un batiment
    public void Build(string name)
    {
        Building newBuilding = BuildingBank.CreateBuilding(name); // Trouver le building
        buildings.Add(newBuilding); // L'ajouter a la liste de building construit
        buildingsName.Add(name);

        if (newBuilding.HasBehavior())
        {
            BuildingBehavior behavior = newBuilding.CreateBehavior();
            behaviors.Add(name, behavior);
            behavior.OnBuild();
        }

        newBuilding.Apply(myVillage); // Appliquer la construction du building
    }
}
