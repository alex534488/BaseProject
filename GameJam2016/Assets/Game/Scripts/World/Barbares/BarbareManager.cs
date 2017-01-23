﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Utility;

public class BarbareManager : INewDay
{
    static public int TEAM = 2;

    private List<BarbarianClan> listClans = new List<BarbarianClan>();

    // A modifier par un script a part qui gere les difficultes
    List<int> listSpawnPoint = new List<int>();
    private int spawnAttackPower = 1;
    private int spawnAttackCooldown = 1;
    private int spawnRate = 1;
    private int spawnCoolDown = 5;
    private int spawnCounter;
    private int count = 0;
    private int stepMultiplier = 5;

    // A faire en debut de partie pour initialiser les barbres 
    public BarbareManager()
    {
        listSpawnPoint.Add(0);
        listSpawnPoint.Add(7);

        // Spawn un barbare pour débuter la partie
        Spawn(1, listSpawnPoint);

        spawnCounter = spawnCoolDown;
    }

    public void NewDay()
    {
        //TODO: Ajuster en fonctione de la difficulte
        if(spawnCounter <= 0)
        {
            Spawn(spawnRate, listSpawnPoint);
            spawnCounter = spawnCoolDown;
            count++;
        } else
        {
            spawnCounter--;
        }
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
            if (listClans[i].GetPosition() == position)
            {
                result.Add(listClans[i]);
            }
        }
        return result;
    }

    public void Spawn(int amount, List<int> positions)
    {
        List<int> listPoint = new List<int>();
        if (positions == null)
        {
            listPoint = listSpawnPoint;
        }
        else
        {
            listPoint = positions;
        }

        Lottery lot = new Lottery();
        for (int i = 0; i < listPoint.Count; i++)
        {
            lot.Add(listPoint[i], 1);
        }

        for (int i = 0; i < amount; i++)
        {
            BarbarianClan newBarbare = new BarbarianClan(spawnAttackPower, spawnAttackCooldown, (int)lot.Pick());
            listClans.Add(newBarbare);
        }
    }

    public void Delete(BarbarianClan clan)
    {
        for (int i = 0; i < listClans.Count; i++)
        {
            if (listClans[i] == clan)
            {
                listClans.Remove(clan);
            }
        }
    }
}
