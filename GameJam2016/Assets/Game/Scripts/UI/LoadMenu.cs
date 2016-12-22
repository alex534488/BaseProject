using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class LoadMenu : MonoBehaviour {

    public Button button;
    public string SceneName = "LoadMenu";

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //load screen
        Scenes.LoadAsync(SceneName, LoadSceneMode.Additive);
    }
}
