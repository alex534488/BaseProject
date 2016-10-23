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
            estimation += EstimationVillage(unEmpire.listVillage[i], nbSoldats);
        }

        estimation += EstimationCapitale(capitale, nbSoldats, nbBarbares);

        return ((float)estimation)/150;
    }

    private static int EstimationVillage (Village leVillage, int nbSoldats)
    {
        int estimation = 0;
        if (leVillage.isDestroyed == true)
        {
            estimation -= 10;
            return estimation;
        }

        int bilanFood = leVillage.coutNourriture - leVillage.nourriture;
        int soldatVillage = leVillage.army;

        nbSoldats = nbSoldats + soldatVillage;

        if (bilanFood < 0)
        {
            estimation -= 3;
        }

        int bilanGold = leVillage.or;

        int reputation = leVillage.reputation;

        if (reputation <= 75)
        {
            if (reputation <= 50)
            {
                if (reputation <= 25)
                    estimation -= 5;

                else
                    estimation -= 3;
            }

            else
                estimation -= 1;
        }

        if (leVillage.isAttacked == true)
        {
            int barbareVillage = leVillage.lord.seuilArmy;

            int differenceForce = soldatVillage - barbareVillage;

            if (differenceForce < 0)
            {
                estimation -= 2;
            }
        }
        return estimation;
    }

    private static int EstimationCapitale(Village laCapitale, int nbSoldats, int nbBarbares)
    {
        int estimation = 0;
        int bilanFood = laCapitale.productionNourriture - laCapitale.coutNourriture;
        int bilanGold = laCapitale.or;

        if (bilanGold < 0)
        {
            estimation -= 15;
        }

        if (bilanFood < 0)
        {
            estimation -= 15;
        }

        int bilanForce = nbSoldats - nbBarbares;

        if (bilanForce < 0)
        {
            estimation -= 15;
        }
        return estimation;
    }
}
