using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) DayManager.main.Lose("Tu t'est simplement fait pété les chevilles.");
    }
    
}
