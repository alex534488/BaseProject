using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayDisplay : MonoBehaviour
{

    public Text text;

    void Start()
    {
        DayManager.OnNewDay.AddListener(UpdateDisplay);
    }

    void UpdateDisplay()
    {
        string extention = "th day";
        if (Universe.World.CurrentDay % 10 == 1)
            extention = "st day";
        else if (Universe.World.CurrentDay % 10 == 2)
            extention = "nd day";
        text.text = Universe.World.CurrentDay + extention;
    }
}
