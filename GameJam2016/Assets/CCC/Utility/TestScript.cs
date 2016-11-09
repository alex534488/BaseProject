﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
            frame.CustomFrames(0);
            RequestManager.SendRequest(frame.Build(Empire.instance.listVillage[0], Empire.instance.capitale));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {

        }
    }

}
