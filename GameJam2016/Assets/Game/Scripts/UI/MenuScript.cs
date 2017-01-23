using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using CCC.Manager;
using CCC.UI;

public class MenuScript : MonoBehaviour {

    private static bool fade = true;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" && fade) MainSceneFade.instance.FadeIn();
        fade = true;
    }

    public void LoadScene(string name)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") MainSceneFade.instance.FadeOut(OnExitMenuToGameComplete);
        else SceneManager.LoadScene(name);
    }

    void OnExitMenuToGameComplete()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void LoadWindow(string name)
    {
        Scenes.LoadAsync(name, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        MainSceneFade.instance.FadeOut();
        Application.Quit();
    }

    public void Return()
    {
        fade = false;

        SceneManager.LoadScene(name);
    }

    public void SetMode(string name)
    {
        ModeManager.modeManager.SetCurrentMode(name);
    }
}
