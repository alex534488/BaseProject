using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : IUpdate {

    public static World main;
    public Empire empire;

	public void Start ()
    {
        if (main == null) main = this;

        empire = new Empire();
        empire.Start();
    }

    public void Update()
    {
        empire.Update();
    }

    public Village GiveTarget() // Verifie la liste des villages et retourne le village frontiere le plus faible de la liste
    {
        float minimalArmy = Mathf.Infinity;
        Village bestTarget = null;

        foreach (Village leVillage in empire.listVillage)
        {
            if (leVillage.army <= minimalArmy) // && Village est une frontiere
            {
                bestTarget = leVillage;
                minimalArmy = leVillage.army;
            }
        }

        return bestTarget;
    }
}
