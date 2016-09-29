using UnityEngine;
using System.Collections;
using System;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    void OnMouseDown()
    {
        Vector3 v3 = Input.mousePosition;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        print("pos: " + v3);
    }
}
