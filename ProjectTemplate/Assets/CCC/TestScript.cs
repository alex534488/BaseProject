using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using FullInspector;

public class TestScript : BaseBehavior
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Stopwatch stopwatch = new Stopwatch(Stopwatch.PrintType.Milliseconds);

            for (int i = 0; i < 60000; i++)
            {
                i = i - 1 + 1;
            }

            stopwatch.Print();
        }
    }
}
