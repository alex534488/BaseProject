using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Conseiller : MonoBehaviour
{

    public Ressource_Type requestType;
    public Button button;

    RessourceManager manager;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //load screen
        SceneManager.LoadScene(ConseillerScreen.SCENE, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ConseillerScreen screen = scene.GetRootGameObjects()[0].GetComponent<ConseillerScreen>();
        screen.Init(World.main.empire.listVillage, requestType);
    }

}
