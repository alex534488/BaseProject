using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class Building : MonoBehaviour
{
    // Nom
    [Tooltip("Nom du batiment")]
    public string nom = "default";

    // Description
    [Tooltip("Description du batiment")]
    public string description = "utilite du batiment ici";

    // Batiment de village?
    [Tooltip("Le batiment peut etre construit dans les villages")]
    public bool IsAvailableInVillage = false;

    // Batiment pour la capitale?
    [Tooltip("Le batiment peut etre construit dans la capitale")]
    public bool IsAvailableInCapital = false;

    // Behavior du Building
    [Tooltip("Script du comportement du building")]
    public BuildingBehavior buildingBehavior;

    // Modificateur de Stats
    public List<VillageModifier> villageModifierList;
    public List<EmpireModifier> empireModifierList;
    
    // Obtenir le nom du batiment
    public string GetName()
    {
        return nom;
    }

    // Applique les modifications necessaire lors de l'achat du batiment
    public void Apply(Village village)
    {
        for(int i = 0; i < villageModifierList.Count; i++)
        {
            villageModifierList[i].ApplyModifier(village);
        }
        for (int i = 0; i < empireModifierList.Count; i++)
        {
            empireModifierList[i].ApplyModifier(Universe.Empire);
        }
    }
}

