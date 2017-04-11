using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using FullInspector;

public class TestScript : Bank<GameObject>
{
    protected override string Convert(GameObject obj)
    {
        if (obj == null)
            return "null";
        return obj.name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestScript.Add(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestScript.Remove(gameObject);
        }
    }
}
