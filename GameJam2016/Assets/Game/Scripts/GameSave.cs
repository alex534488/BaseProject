﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CCC.Utility;

[System.Serializable]
public class GameSave
{
    public string name;
    public World world;
    public History history;

    //public RequestManagerSave
    //public StorylineSave
    //etc.

    public GameSave(string name, World world, History history)//, history, requestManagerSave, etc.
    {
        this.name = name;
        this.world = world;
        this.history = history;
    }

    static public GameSave Load(string gameName)
    {
        if (!Exists(gameName))
            return null;

        string path = GetBaseFilePath() + gameName + ".dat";

        return (GameSave)Saves.InstantLoad(path);
    }

    static public void Save(GameSave save)
    {
        string path = GetBaseFilePath() + save.name + ".dat";
        Saves.InstantSave(path, save);
    }

    static public bool Exists(string gameName)
    {
        string path = GetBaseFilePath() + gameName + ".dat";
        return Saves.Exists(path);
    }

    static public string GetBaseFilePath()
    {
        return Application.persistentDataPath + "/";
    }

    static public string GetGameFilePath()
    {
        return Application.persistentDataPath + "/" + GameManager.currentGameName + "_";
    }
}
