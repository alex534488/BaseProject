using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class History
{
    List<World> worlds = new List<World>();

    public void RecordDay(World world)
    {

    }

    public void LoadPast(int days)
    {

    }

    public World ViewPast(int days)
    {
        if (days > worlds.Count || days <= 0)
            return null;

        return null;
    }
}
