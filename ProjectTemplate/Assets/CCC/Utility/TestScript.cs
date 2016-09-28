using UnityEngine;
using System.Collections;
using System;
using CCC.Manager;

public class TestScript : MonoBehaviour
{
    void Awake()
    {
        MasterManager.Sync(Init);
    }
    void Init()
    {
        DelayManager.CallTo(delegate () { print("Helloworld."); }, 5 );
    }
}
