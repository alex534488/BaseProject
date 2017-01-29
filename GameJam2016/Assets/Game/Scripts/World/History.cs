﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

[System.Serializable]
public class History
{
    private int worldCountLimit = 2;
    List<World> worlds = new List<World>();

    public void RecordDay(World world)
    {
        if (worlds.Count >= worldCountLimit)
            worlds.RemoveAt(0);

        worlds.Add(ObjectCopier.Clone(world));
    }

    public void LoadPast(int days)
    {
        World world = ViewPast(days);

        if (world != null)
            Universe.instance.SetWorldTo(world);
    }

    public World ViewPast(int days)
    {
        if (days >= worlds.Count || days < 0)
            return null;

        int index = (worlds.Count - 1) - days;

        return worlds[index];
    }
}
