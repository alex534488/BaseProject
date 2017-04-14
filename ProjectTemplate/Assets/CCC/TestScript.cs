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
        return obj.name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print(GetRandom());
        }
    }
}
