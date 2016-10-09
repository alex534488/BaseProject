using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : IUpdate {

    public static World main;
    public Empire empire;

    public BarbareManager barbareManager;

    public World()
    {
        barbareManager = new BarbareManager();
        barbareManager.Initialize();
        empire = new Empire();
        main = this;
        empire.Start();
    }

    public void Start ()
    {


       
       
    }

    public void Update()
    {
        empire.Update();
        barbareManager.Uptade();
    }

    public Village GiveTarget() // Verifie la liste des villages et retourne le village frontiere le plus faible de la liste
    {
        float minimalArmy = Mathf.Infinity;
        Village bestTarget = null;

        foreach (Village leVillage in empire.listVillage)
        {
            if (leVillage.army <= minimalArmy && leVillage.isFrontier)
            {
                bestTarget = leVillage;
                minimalArmy = leVillage.army;
            }
        }

        return bestTarget;
    }
}
