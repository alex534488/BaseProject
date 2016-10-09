using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BarbareManager : IUpdate {

    public List<Barbare> listeBarbare = new List<Barbare>();

   
    public int nbClanBarbare = 1;

    

    public void Initialize()
    {
        for(int i = 0; i < nbClanBarbare; i++)
        {
            AddClanBarbare();
        }
	}

    public void Uptade()
    {
        foreach(Barbare bar in listeBarbare)
        {
            bar.Update();
        }
    }

    public void spawn()
    {
        
    }

    public void AddClanBarbare()
    {
        Barbare unBarbare = new Barbare();
        listeBarbare.Add(unBarbare);
    }

    public int NombreTotalBarbare()
    {
        int barbareTotal = 0;

        for (int i = 0; i<listeBarbare.Count; i++)
        {
            barbareTotal = barbareTotal + listeBarbare[i].nbBarbares;
        }

        return barbareTotal;
    }

   

    public void Update()
    {
        throw new NotImplementedException();
    }
}
