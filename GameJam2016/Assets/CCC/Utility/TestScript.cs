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

[System.Serializable]
public class TestSave
{
    public Stat<int> b = new Stat<int>(5, 0, 10, BoundMode.Cap);
}

public class TestScript : MonoBehaviour
{
    public Text text;
    public RequestFrame frame;
    public Camera cam;
    public int speed = 10;
    TestSave save = new TestSave();

    void Start()
    {
        save.b.onMaxReached.AddListener(MaxReached);
        save.b.onMinReached.AddListener(MinReached);
    }

    void MaxReached(int value)
    {
        print("max reached");
    }
    void MinReached(int value)
    {
        print("min reached");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L)) DayManager.main.Lose("Tu t'est simplement fait pété les chevilles.");
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            save.b.Set(save.b + 1);
            print("b = " + save.b);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            save.b.Set(save.b - 1);
            print("b = " + save.b);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //LocalSave();
            ThreadSave.Save(GameSave.GetFilePath() + "test.dat", save, OnSaveComplete);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ThreadSave.Delete(GameSave.GetFilePath() + "test.dat");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ThreadSave.Load(GameSave.GetFilePath() + "test.dat", OnLoadComplete);
        }
    }

    void OnSaveComplete()
    {
        print("saved");
    }
    void OnLoadComplete(object graph)
    {
        save = (TestSave)graph;
        print("loaded");
        print("b = " + save.b);
    }

}
