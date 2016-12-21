using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    public Text text;
    public RequestFrame frame;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) DayManager.main.Lose("Tu t'est simplement fait pété les chevilles.");
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            StorylineManager.Launch<DemoStory>();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StorylineManager.Terminate<DemoStory>();
        }
    }

}
