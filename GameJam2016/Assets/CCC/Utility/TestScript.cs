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
    IKit kit = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StorylineManager.Launch<DemoStory>();
            //Universe.History.LoadPast(2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            RequestManager.SendRequest((Request)Saves.InstantLoad(Application.persistentDataPath + "/rq.dat"));
            //GameManager.LoadGame(GameManager.GetGameSaves()[0]);
            print("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            List<Choice> choix = new List<Choice>
            {
                new Choice("Choix 1", new Command(CommandType.Print, "click sur choix 1")),
                new Choice("Choix 2", new Command(CommandType.Print, "click sur choix 2")),
                new Choice("Choix 3", new Command(CommandType.Print, "click sur choix 3")),
            };
            Request rq = new Request(new Dialog.Message("Texte de presentation"), choix);
            rq.SetCharacterKit(kit);
            Saves.InstantSave(Application.persistentDataPath + "/rq.dat", rq);
            //GameManager.SaveCurrentGame();
            print("Saved");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            kit = CharacterBank.GetRandomKit();
            //GameManager.NewGame();
            print("New Game");
        }
    }

    void Print1()
    {
        print("1");
    }
    void Print2()
    {
        print("2");
    }

}

