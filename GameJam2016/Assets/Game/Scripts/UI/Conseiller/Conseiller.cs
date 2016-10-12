using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class Conseiller : MonoBehaviour
{

    public Ressource_Type requestType;
    public Button button;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //load screen
        Scenes.Load(ConseillerScreen.SCENE, LoadSceneMode.Additive, OnSceneLoaded, true);
    }

    void OnSceneLoaded(Scene scene)
    {
        ConseillerScreen screen = scene.GetRootGameObjects()[0].GetComponent<ConseillerScreen>();
        screen.Init(World.main.empire.listVillage, requestType);
    }

}
