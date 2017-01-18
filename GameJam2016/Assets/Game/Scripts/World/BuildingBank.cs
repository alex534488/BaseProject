using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Stock tous les buildings disponible dans le jeu
public class BuildingBank : Singleton<BuildingBank>
{
    public List<Building> buildings = new List<Building>(); // Liste des buildings disponible

    static public Building CreateBuilding(string name)
    {
        for (int i = 0; i < instance.buildings.Count; i++)
        {
            if (instance.buildings[i].GetName() == name)
            {
                return  Instantiate(instance.buildings[i]) as Building;
            }
        }
        return null;
    }
}
