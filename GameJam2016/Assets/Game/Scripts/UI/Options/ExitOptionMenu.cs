using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;
using CCC.UI;

public class ExitOptionMenu : MonoBehaviour {

    public string SceneName = "OptionMenu";
    public AudioClip clip;
    public WindowAnimation Window = null;
    private bool quit = false;

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
                    if (clip != null) SoundManager.Play(clip);
                    quit = false;
                }
            );
        }
        else
        {
            SceneManager.UnloadScene(SceneName);
            if (clip != null) SoundManager.Play(clip);
            quit = false;
        }

    }
}
