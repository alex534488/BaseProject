using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tech
{
    public Building building = null;
    public bool researched = false;
    public bool visible = true;
    public string Name { get { return building.name; } }
}