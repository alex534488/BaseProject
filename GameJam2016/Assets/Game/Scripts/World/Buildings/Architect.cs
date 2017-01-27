using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class Architect : INewDay
{
    Village myVillage;

    public Architect(Village village)
    {
        myVillage = village;
    }

    // Holy trinity
    [System.NonSerialized]
    private List<Building> buildings = new List<Building>(); // Liste de buildings construit dans le village
    private List<string> buildingsName = new List<string>(); // Liste des NOMS de buildings construit dans le village
    private List<BuildingBehavior> behaviors = new List<BuildingBehavior>();// Dictionnaire contenant tout les behavior particuliere de building

    public ReadOnlyCollection<string> BuildingsName
    {
        get { return buildingsName.AsReadOnly(); }
    }

    //Appelé lorsqu'on load une partie 
    [OnDeserialized]
    public void OnLoad(StreamingContext context)
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
        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].GetName() == name)
            {
                return buildings[i];
            }
        }
        return null;
    }

    public void DestroyBuilding(string name)
    {
        for(int i=0; i<buildingsName.Count; i++)
        {
            if(buildingsName[i] == name)
            {
                DestroyBuildingAt(i);
                return;
            }
        }
    }

    public void DestroyBuildingAt(int position)
    {
        if (position >= buildingsName.Count)
            return;
        buildingsName.RemoveAt(position);
        buildings.RemoveAt(position);
        behaviors.RemoveAt(position);
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
        if (newBuilding == null)
        {
            Debug.LogWarning("Cannot build " + name + " in village. The building does not exist.");
            return;
        }

        buildings.Add(newBuilding); // L'ajouter a la liste de building construit
        buildingsName.Add(name);

        if (newBuilding.HasBehavior())
        {
            BuildingBehavior behavior = newBuilding.CreateBehavior();
            behaviors.Add(behavior);
            behavior.OnBuild();
        }
        else
        {
            behaviors.Add(null);
        }

        newBuilding.Apply(myVillage); // Appliquer la construction du building
    }
}
