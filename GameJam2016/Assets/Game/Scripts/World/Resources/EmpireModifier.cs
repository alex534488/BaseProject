using UnityEngine;
using System.Collections;

public class EmpireModifier : MonoBehaviour {
    public Empire_ResourceType resource = Empire_ResourceType.custom;
    public int amount;

    public void ApplyModifier(Empire empire)
    {
        empire.Add(resource, amount);
    }

    public void DeApplyModifier(Empire empire)
    {
        empire.Remove(resource, amount);
    }
}
