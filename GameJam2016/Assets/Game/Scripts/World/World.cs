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
        barbareManager.Uptade();
        empire.Update();
    }

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
}
