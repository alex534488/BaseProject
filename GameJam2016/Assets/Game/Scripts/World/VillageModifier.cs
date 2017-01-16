﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class VillageModifier {
    public Village_ResourceType resource;
    public int amount;

    public void ApplyModifier(Village village)
    {
        village.Add(resource, amount);
    }
}