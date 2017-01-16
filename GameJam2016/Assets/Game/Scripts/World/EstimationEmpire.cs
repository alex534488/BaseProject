using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstimationEmpire
{
    static float bonheurDepart = -1;
    
    static public float Estimation()
    {
        //if (bonheurDepart == -1)
        //    bonheurDepart = Empire.instance.capitale.GetBonheur();

        //float a = EstimationVillage();
        //float b = EstimationBonheur();

        //if (a <= b)
        //    return a;

        //else
        //    return b;
        return 1;
    }

    private static float EstimationVillage()
    {
        //float nbVillageRestant = Empire.instance.listVillage.Count;
        //return (nbVillageRestant / Empire.instance.nbVillage);
        return 1;
    }

    private static float EstimationBonheur()
    {
        //float nbBonheurRestant = Empire.instance.capitale.GetBonheur();
        //return (nbBonheurRestant / bonheurDepart);
        return 1;
    }
}
