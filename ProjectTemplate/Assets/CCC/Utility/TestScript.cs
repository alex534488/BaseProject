using UnityEngine;
using System.Collections;
using System;
using CCC.Manager;
using CCC.Utility;

public class TestScript : SlowBehaviour
{
    public bool test;
    protected override void SlowUpdate()
    {
        base.SlowUpdate();
        print("hello");
    }
}
