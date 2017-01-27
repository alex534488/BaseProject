using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayDisplay : MonoBehaviour
{
    public int numberSufixSize = 22;
    public Text text;

    void Start()
    {
        DayManager.OnNewDay.AddListener(UpdateDisplay);
        DayManager.SyncToInit(Init);
    }

    void Init()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        string extention = "<size=" + numberSufixSize + ">th</size> day";
        if (Universe.World.CurrentDay % 10 == 1)
            extention = "<size=" + numberSufixSize + ">st</size> day";
        else if (Universe.World.CurrentDay % 10 == 2)
            extention = "<size=" + numberSufixSize + ">nd</size> day";
        text.text = Universe.World.CurrentDay + extention;
    }
}
