using UnityEngine;
using System.Collections;

[System.Serializable]
public class VillageModifier {
    public Village_ResourceType resource = Village_ResourceType.custom;
    public int amount;

    public void ApplyModifier(Village village)
    {
        village.Add(resource, amount);
    }

    public void DeApplyModifier(Village village)
    {
        village.Remove(resource, amount);
    }
}
