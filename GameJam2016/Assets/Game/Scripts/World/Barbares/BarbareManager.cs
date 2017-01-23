using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Utility;

public class BarbareManager : INewDay {

    private List<BarbarianClan> listClans = new List<BarbarianClan>();
    private int spawnAttackPower = 1;
    private int spawnAttackCooldown = 1;

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

    public List<BarbarianClan> GetAllClans()
    {
        return listClans;
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

    public void Spawn(int amount, List<int> positions)
    {
        List<int> listSpawnPoint = new List<int>();
        if (positions == null)
        {
            listSpawnPoint.Add(0);
            listSpawnPoint.Add(7);
        } else
        {
            listSpawnPoint = positions;
        }

        Lottery lot = new Lottery();
        for (int i = 0; i < listSpawnPoint.Count; i++)
        {
            lot.Add(listSpawnPoint[i], 1);
        }

        for(int i = 0; i < amount; i++)
        {
            BarbarianClan newBarbare = new BarbarianClan(spawnAttackPower, spawnAttackCooldown, (int)lot.Pick());
            listClans.Add(newBarbare);
        }
    }

    public void Delete(BarbarianClan clan)
    {
        for(int i = 0; i < listClans.Count; i++)
        {
            if(listClans[i] == clan)
            {
                listClans.Remove(clan);
            }
        }
    }
}
