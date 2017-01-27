using UnityEngine;
using System.Collections;
using CCC.Manager;
using System.Collections.Generic;

public class GameManager : BaseManager
{
    private static int maxGameCount = 3;
    public static int MaxGameCount { get { return maxGameCount; } }
    private static string gameNamePrefix = "game";
    private static GameSave loadingSave = null;
    static public string currentGameName = "";
    //static public string CurrentGameName { get { return ((GameManager)instance).currentGameName; } }

    public override void Init()
    {
        base.Init();

        if (Scenes.Exists("Main"))
            DayManager.main.Init();

        CompleteInit();
    }

    static public bool NewGame()
    {
        string gameName = GetAvailableGameName();
        if (string.IsNullOrEmpty(gameName))
            return false;

        currentGameName = gameName;
        LaunchGame(null);

        return true;
    }

    //TODO
    static public bool LoadGame(string saveName)
    {
        GameSave save = GameSave.Load(saveName);
        if (save == null)
            return false;

        currentGameName = save.name;
        LaunchGame(save);

        return true;
    }

    static private void LaunchGame(GameSave save = null)
    {
        /*if (Scenes.Exists("Main"))
            Scenes.Unload("Main");*/
        loadingSave = save;
        Scenes.Load("Main", UnityEngine.SceneManagement.LoadSceneMode.Single, LaunchGameComplete, false);
    }

    static private void LaunchGameComplete(UnityEngine.SceneManagement.Scene scene)
    {
        DayManager.main.Init(loadingSave);
    }
    
    static public bool SaveCurrentGame()
    {
        if (!Scenes.Exists("Main") || DayManager.main == null || DayManager.main.Universe == null)
            return false;

        if (string.IsNullOrEmpty(currentGameName))
        {
            Debug.LogWarning("Cannot save game. currentGameName = null or empty");
            return false;
        }

        GameSave save = new GameSave(currentGameName, DayManager.main.Universe.world);

        GameSave.Save(save);

        return true;
    }


    static public List<string> GetGameSaves()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < maxGameCount; i++)
            if (GameSave.Exists(gameNamePrefix + i.ToString()))
                list.Add(gameNamePrefix + i.ToString());
        return list;
    }

    static public List<string> GetAllGameNames()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < maxGameCount; i++)
            list.Add(gameNamePrefix + i.ToString());
        return list;
    }


    static public string GetAvailableGameName()
    {
        List<string> possibilities = GetAllGameNames();
        List<string> taken = GetGameSaves();

        foreach (string potentialName in possibilities)
        {
            bool available = true;
            foreach (string takenName in taken)
            {
                if (takenName.Equals(potentialName))
                {
                    available = false;
                    break;
                }
            }
            if (available)
                return potentialName;
        }

        return null;
    }

}
