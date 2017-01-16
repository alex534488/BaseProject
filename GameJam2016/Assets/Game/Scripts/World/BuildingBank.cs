using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Stock tous les buildings disponible dans le jeu
public class BuildingBank : Singleton<BuildingBank>
{
    public List<Building> buildings = new List<Building>(); // Liste des buildings disponible
    public GameObject buildingHolder;

    static public Building GetBuilding(string name, Village village)
    {
        for (int i = 0; i < instance.buildings.Count; i++)
        {
            if (instance.buildings[i].GetName() == name)
            {
                // Batiment trouvé!
                GameObject currentHolder; // référence au parent du game object du batiment
                foreach (Transform child in instance.buildingHolder.transform)
                {
                    // Es ce que le parent existe deja dans le buildingHolder?
                    if(child.name == village.Name)
                    {
                        // Si oui on ajuste la référence
                        currentHolder = child.gameObject;
                    }
                }
                if(currentHolder = null)
                {
                    // Si non on le crée
                    currentHolder = new GameObject();
                    currentHolder.name = village.Name;
                    currentHolder.transform.SetParent(instance.buildingHolder.transform);
                }
                // Création du gameobject du batiment dans la scène en le placant au bon endroit;
                GameObject building = Instantiate(instance.buildings[i].gameObject);
                building.transform.SetParent(currentHolder.transform);
                return building.GetComponent<Building>();
            }
        }
        return null;
    }
}
