using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : INewDay {

    public static World main;

    public Empire empire;
    public BarbareManager barbareManager;
    public Map map;

    // Creation du monde du jeu
    public World()
    {
        main = this;

        // Creation des barbares
        barbareManager = new BarbareManager();
        barbareManager.Initialize();

        // Creation de l'empire
        empire = new Empire();
        empire.Start();

        // Creation de la map
        map = new Map();
        map.Start();
    }

    public void NewDay()
    {
        barbareManager.Uptade();
        empire.NewDay();
    }

    // TODO: Refaire cette fonctione pour qu'elle utilise la map
    public List<Village> GiveTarget() // Verifie la liste des villages et retourne le village frontiere le plus faible de la liste
    {
        List<Village> ret = new List<Village>();
        List<Village> clone = new List<Village>(empire.listVillage);

        for(int i=0;i<empire.listVillage.Count;i++)
        {
            float minimalArmy = Mathf.Infinity;
            Village bestTarget = clone[0];

            foreach (Village leVillage in clone)
            {
                if (leVillage.GetArmy() <= minimalArmy && leVillage.isFrontier && leVillage.isAttacked==false)
                {
                    bestTarget = leVillage;
                    minimalArmy = leVillage.GetArmy();
                }
            }
            ret.Add(bestTarget);
            clone.Remove(bestTarget);
        }
        

        return ret;
    }

    // TODO: Faire la fonctione qui copie world pour en faire une sauvegarde
    public World CloneState()
    {
        return this;
    }
}
