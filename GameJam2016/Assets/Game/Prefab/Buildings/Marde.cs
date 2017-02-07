using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

[System.Serializable]
public class Marde : BuildingBehavior {

    int val = 0;

    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {
        //Debug.Log("val: " + val);
    }

    public override void OnBuild()
    {
        val = 10;
        Debug.Log("Printerinosaurus Rekt");
    }
}
