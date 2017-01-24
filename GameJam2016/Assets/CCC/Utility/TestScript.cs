﻿using UnityEngine;
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
            //Transaction t1 = new Transaction(new Village("Toma"), new Village("Boby"), ResourceType.science, 10, Transaction.ValueType.sourcePercent);
            //Cart cart = new Cart(2, 5, 6, new List<Transaction>() { t1 });
            //Universe.CartsManager.SendCart(cart);
            //save = new B(1, 2);
            //print(save.GetType());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LocalSave();
            //ThreadSave.Save(GameSave.GetFilePath() + "test.dat", save, OnSaveComplete);
        }
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    ThreadSave.Delete(GameSave.GetFilePath() + "test.dat");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
            //ThreadSave.Load(GameSave.GetFilePath() + "test.dat", OnLoadComplete);
        //}
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

