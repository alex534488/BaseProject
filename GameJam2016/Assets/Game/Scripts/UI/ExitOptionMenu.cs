using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class ExitOptionMenu : MonoBehaviour {

    public string SceneName = "OptionMenu";

    public void Exit()
    {
        SceneManager.UnloadScene(SceneName);
    }
}
