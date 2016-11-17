using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class ExitOptionMenu : MonoBehaviour {

    public string SceneName = "OptionMenu";
    public AudioClip clip;

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
        SceneManager.UnloadScene(SceneName);
        if(clip != null) SoundManager.Play(clip);
    }
}
