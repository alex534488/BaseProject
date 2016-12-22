using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;
using CCC.UI;

public class ExitLoadMenu : MonoBehaviour {

    public string SceneName = "LoadGameScene";
    public WindowAnimation Window = null;
    private bool quit = false;

    public void Cancel()
    {
        Exit();
    }

    public void Load()
    {
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
