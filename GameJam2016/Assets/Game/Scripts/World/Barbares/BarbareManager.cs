using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BarbareManager : INewDay {

    private List<BarbarianClan> listClans = new List<BarbarianClan>();

    public void Init()
    {
        // A faire en debut de partie pour initialiser les barbres 
        // Utilise Spawn
    }

    public void NewDay()
    {
        //TODO: IA qui prend des decisions sur le positionnement et l'appartition des barbares
        // Utilise Spawn et Send
    }

    public List<BarbarianClan> GetClans(int position)
    {
        List<BarbarianClan> result = new List<BarbarianClan>();
        for (int i = 0; i < listClans.Count; i++)
        {
            if(listClans[i].GetPosition() == position)
            {
                result.Add(listClans[i]);
            }
        }
        return result;
    }

    private void Spawn(int amount, List<int> positions)
    {
        // TODO: Repartitions de la quantite sur tous les territoires de facon le plus equitable et aleatoire
    }
}
