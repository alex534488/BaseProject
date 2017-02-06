using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;
using CCC.Manager;
using CCC.Utility;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Fonction
{
    public static void TestFonction()
    {
        Debug.Log("1");

    }
}

[System.Serializable]
public class A
{
    int number = 5;
    public UnityAction a = null;
    public A(UnityAction a)
    {
        this.a = a;
    }

    public void Exectute()
    {
        a();
    }
}

[System.Serializable]
public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("Example");
        }
    }
}

