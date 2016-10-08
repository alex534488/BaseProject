using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstimationEmpire : IUpdate {

    public World unWorld;
    public Empire unEmpire;

    public void Initalize()
    {
        
    }

    public void Update ()
    {
        Estimation();
	}

    public float Estimation()
    {
        Village capitale = unEmpire.capitale;

        int estimation = 150;

        int nbVillages = unEmpire.listVillage.Count;
        int nbSoldats = 0;
        int nbBarbares = 0;
       
        for (int i=0; i< nbVillages;i++)
        {
            EstimationVillage(unEmpire.listVillage[i], estimation, nbSoldats);
        }

        EstimationCapitale(capitale, estimation, nbSoldats, nbBarbares);

        return ((float)estimation/150);
    }

    private void EstimationVillage (Village leVillage, int estimation, int nbSoldats) // To do : Enlever commentaires 
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

        int reputation = 0; // leVillage.lord.reputation;

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
            int barbareVillage = 0; // leVillage.lord.seuilarmy

            int differenceForce = soldatVillage - barbareVillage;

            if (differenceForce < 0)
            {
                estimation = estimation - 2;
            }
        }
    }

    private void EstimationCapitale(Village laCapitale, int estimation, int nbSoldats, int nbBarbares)
    {
        int bilanFood = laCapitale.productionNourriture - laCapitale.coutNourriture;

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
