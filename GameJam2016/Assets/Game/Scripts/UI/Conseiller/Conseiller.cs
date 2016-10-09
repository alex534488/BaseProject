using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Conseiller : MonoBehaviour
{

    public Ressource_Type requestType;
    public Button button;

    bool loading = false;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        button.GetComponent<AudioSource>().Play();

        if (loading) return;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == ConseillerScreen.SCENE) return;
        }

        //load screen
        loading = true;
        SceneManager.LoadScene(ConseillerScreen.SCENE, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoading;
    }

    IEnumerator WaitForSceneLoad(Scene scene)
    {
        while (!scene.isLoaded) yield return null;
        OnSceneLoaded(scene);
    }
    void OnSceneLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoading;
        if (!scene.isLoaded) StartCoroutine(WaitForSceneLoad(scene));
        else OnSceneLoaded(scene);
    }

    void OnSceneLoaded(Scene scene)
    {
        loading = false;
        ConseillerScreen screen = scene.GetRootGameObjects()[0].GetComponent<ConseillerScreen>();
        screen.Init(World.main.empire.listVillage, requestType);
    }

}
