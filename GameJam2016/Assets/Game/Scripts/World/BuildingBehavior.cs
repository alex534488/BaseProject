using UnityEngine;
using System.Collections;

//// Classe abstraire qui doit être hérité des scripts associés aux buildings
//public interface IBuildingBehavior
//{
//    bool OnBuy();     // action a faire lors d'un achat
//    bool OnDestroy(); // action a faire lors d'une destruction
//    bool OnNewDay();  // action a faire lors d'une nouvelle journee
//    // N.B. Retourner true seulement si on veut que la fonction s'effectue sans le Apply par default du batiment
//}
// Classe par default de buildingBehavior
[System.Serializable]
public class BuildingBehavior //: IBuildingBehavior
{
    public virtual void OnBuild()
    {
    }

    public virtual void OnDestroy()
    {
    }

    public virtual void OnNewDay()
    {
    }
}
