﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CCC.Manager;
using CCC.UI;
using UnityEngine.Events;

public class ExitLoadMenu : MonoBehaviour {

    public string SceneName = "LoadGameScene";
    public WindowAnimation Window = null;
    public Button displayButton;
    private bool quit = false;
    private GameSave currentGameSave;

    public UnityEvent OnSave = new UnityEvent();

    void Start()
    {
        if (displayButton == null) return;

        if (!Scenes.Exists("Main"))
        {
            displayButton.interactable = false;
        }
        else
        {
            displayButton.interactable = true;
        }
    }

    public void Cancel()
    {
        Exit();
    }

    public void Load()
    {
        GameManager.LoadGame(currentGameSave.name);
    }

    public void Save()
    {
        GameManager.SaveCurrentGame();
        OnSave.Invoke();
    }

    public void SetCurrentGameSave(GameSave currentGameSave)
    {
        this.currentGameSave = currentGameSave;
    }

    public void Exit()
    {
        if (quit) return;

        quit = true;

        if (Window != null)
        {
            Window.Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SceneName);
                    quit = false;
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SceneName);
            quit = false;
        }

    }
}
