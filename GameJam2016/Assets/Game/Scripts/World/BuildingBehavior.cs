using UnityEngine;
using System.Collections;

// Classe abstraire qui doit être hérité des scripts associés aux buildings
public interface IBuildingBehavior
{
    bool OnBuy();     // action a faire lors d'un achat
    bool OnDestroy(); // action a faire lors d'une destruction
    bool OnNewDay();  // action a faire lors d'une nouvelle journee
    // N.B. Retourner true seulement si on veut que la fonction s'effectue sans le Apply par default du batiment
}
// Classe par default de buildingBehavior
public class BuildingBehavior : IBuildingBehavior
{
    public bool OnBuy()
    {
        return false;
    }

    public bool OnDestroy()
    {
        return false;
    }

    public bool OnNewDay()
    {
        return false;
    }
}
