using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BarbareManager : INewDay {
    /*
    public List<Barbare> listeBarbare = new List<Barbare>();

   
    public int nbClanBarbare = 2;

    public void Initialize()
    {
        for(int i = 0; i < nbClanBarbare; i++)
        {
            AddClanBarbare();
        }
	}

    public void Update()
    {
        spawn();
        foreach(Barbare bar in listeBarbare)
        {
            bar.NewDay();
        }
    }

    public void spawn()
    {
        if(DayManager.main.nbJour > 0 &&  DayManager.main.nbJour%5 == 0)
        {
            nbClanBarbare++;
            AddClanBarbare();
        }
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

   */

    public void NewDay()
    {
        //TODO
    }
}
