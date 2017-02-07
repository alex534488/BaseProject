using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Science/TechLine")]
public class TechLine : ScriptableObject
{
    public bool unlockByOrder = true;
    public List<Tech> techs = new List<Tech>();
}
