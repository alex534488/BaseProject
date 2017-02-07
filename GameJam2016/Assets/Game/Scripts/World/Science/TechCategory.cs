using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Science/TechCategory")]
public class TechCategory : ScriptableObject
{
    public bool unlockByOrder = true;
    public List<TechLine> techLines = new List<TechLine>();
}
