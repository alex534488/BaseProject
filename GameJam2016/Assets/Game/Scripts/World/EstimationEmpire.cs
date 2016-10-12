using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstimationEmpire {

    static World unWorld;
    static Empire unEmpire;

    static public float Estimation()
    {
        unEmpire = Empire.instance;
        unWorld = World.main;
        Village capitale = unEmpire.capitale;

        int estimation = 150;

        int nbVillages = unEmpire.listVillage.Count;
        int nbSoldats = 0;
        int nbBarbares = unWorld.barbareManager.NombreTotalBarbare();
       
        for (int i=0; i< nbVillages;i++)
        {
            EstimationVillage(unEmpire.listVillage[i], estimation, nbSoldats);
        }

        EstimationCapitale(capitale, estimation, nbSoldats, nbBarbares);

        return ((float)estimation/150);
    }

    private static void EstimationVillage (Village leVillage, int estimation, int nbSoldats)
    {
        if (leVillage.isDestroyed == true)
        {
            estimation = estimation - 10;
            return;
        }

        int bilanFood = leVillage.coutNourriture - leVillage.nourriture;
        int soldatVillage = leVillage.army;

        nbSoldats = nbSoldats + soldatVillage;

        if (bilanFood < 0)
        {
            estimation = estimation - 3;
        }

        int bilanGold = leVillage.or;

        int reputation = leVillage.reputation;

        if (reputation <= 75)
        {
            if (reputation <= 50)
            {
                if (reputation <= 25)
                    estimation = estimation - 5;

                else
                    estimation = estimation - 3;
            }

            else
                estimation = estimation - 1;
        }

        if (leVillage.isAttacked == true)
        {
            int barbareVillage = leVillage.lord.seuilArmy;

            int differenceForce = soldatVillage - barbareVillage;

            if (differenceForce < 0)
            {
                estimation = estimation - 2;
            }
        }
    }

    private static void EstimationCapitale(Village laCapitale, int estimation, int nbSoldats, int nbBarbares)
    {
        int bilanFood = laCapitale.productionNourriture - laCapitale.coutNourriture;
        int bilanGold = laCapitale.or;

        if (bilanGold < 0)
        {
            estimation = estimation - 15;
        }

        if (bilanFood < 0)
        {
            estimation = estimation - 15;
        }

        int bilanForce = nbSoldats - nbBarbares;

        if (bilanForce < 0)
        {
            estimation = estimation - 15;
        }
    }
}
