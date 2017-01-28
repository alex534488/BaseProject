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

[System.Serializable]
public class B : A
{
    public int b = 0;
    public B(int a, int b) : base(a)
    {
        this.b = b;
    }
}
[System.Serializable]
public class A
{
    public int a = 0;
    public A(int a)
    {
        this.a = a;
    }
}

public class TestScript : MonoBehaviour
{
    public A save;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.LoadGame(GameManager.GetGameSaves()[0]);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.SaveCurrentGame();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.NewGame();
        }
    }
    void LocalSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GameSave.GetGameFilePath() + "test.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, Universe.World);
        file.Close();
        OnSaveComplete();
    }

    void OnSaveComplete()
    {
        print("saved");
    }
    void OnLoadComplete(object graph)
    {
        save = (A)graph;
        print("loaded");
        print(save.GetType() + "" + ((B)save).b);
    }

}

