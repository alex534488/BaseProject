using UnityEngine;
using System.Collections;

public class EmpireModifier : MonoBehaviour {
    public Empire_ResourceType resource;
    public int amount;

    public void ApplyModifier(Empire empire)
    {
        empire.Add(resource, amount);
    }
}
