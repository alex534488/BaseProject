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
    Stat<int> stat = new Stat<int>(5);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StorylineManager.Launch<DemoStory>();
            print("Launching Demo storyline");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RequestManager.SendRequest((Request)Saves.InstantLoad(Application.persistentDataPath + "/rq.dat"), 1);
            print("request sent. 1 day of delay");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Universe.History.LoadPast(4);
            print("Rollback 4 days");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.LoadGame(GameManager.GetGameSaves()[0]);
            print("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.SaveCurrentGame();
            print("Saved");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.NewGame();
            print("New Game");
        }
    }
}

