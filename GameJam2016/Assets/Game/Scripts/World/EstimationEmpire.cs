using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstimationEmpire
{

    static float bonheurDepart = -1;

    static World unWorld;
    static Empire unEmpire;

    static public float Estimation()
    {
        unEmpire = Empire.instance;
        unWorld = World.main;

        if (bonheurDepart == -1)
            bonheurDepart = unEmpire.capitale.GetBonheur();

        float a = EstimationVillage();
        float b = EstimationBonheur();

        if (a <= b)
            return a;

        else
            return b;

    }

    private static float EstimationVillage()
    {
        float nbVillageRestant = unEmpire.listVillage.Count;
        return (nbVillageRestant / unEmpire.nbVillage);

    }

    private static float EstimationBonheur()
    {
        float nbBonheurRestant = unEmpire.capitale.GetBonheur();
        return (nbBonheurRestant / bonheurDepart);
    }
}
