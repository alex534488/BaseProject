using UnityEngine;
using System.Collections;
using CCC.Manager;

public class MapWindow : MonoBehaviour {

    public void Open()
    {
        Scenes.Load("Map", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void Close()
    {
        Scenes.UnloadScene("Map");
    }
}
