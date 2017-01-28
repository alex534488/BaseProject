using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;
using CCC.UI;

public class ExitOptionMenu : MonoBehaviour {

    public string SceneName = "OptionMenu";
    public WindowAnimation Window = null;
    private bool quit = false;
    public Button abandonButton;

    public void Awake()
    {
        if (abandonButton == null) return;

        if (SceneManager.GetActiveScene().name != "Main")
        {
            abandonButton.interactable = false;
        } else
        {
            abandonButton.interactable = true;
        }
    }

    public void Credits()
    {
        // load scene credits
    }

    public void LeaderBoard()
    {
        // load scene leaderboard
    }

    public void Abandonner()
    {
        Exit();
        DayManager.main.Lose("Vous avez abandonner votre Empire à sa propre perte.");
    }

    public void LoadGame()
    {
        Scenes.LoadAsync("LoadGameScene", LoadSceneMode.Additive);
    }

    public void Annuler()
    {
        SoundManager.Load();
        Exit();
    }

    public void Confirmer()
    {
        SoundManager.Save();
        Exit();
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
                    SceneManager.UnloadScene(SceneName);
                    quit = false;
                }
            );
        }
        else
        {
            SceneManager.UnloadScene(SceneName);
            quit = false;
        }

    }
}
