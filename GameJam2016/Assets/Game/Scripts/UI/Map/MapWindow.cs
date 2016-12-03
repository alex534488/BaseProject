using UnityEngine;
using System.Collections;
using CCC.Manager;
using CCC.UI;

public class MapWindow : MonoBehaviour
{
    public WindowAnimation mapWindow = null;

    private bool quitting = false;

    public void Open()
    {
        Scenes.Load("Map", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void Close()
    {
        if (quitting)
            return;
        quitting = true;

        if (mapWindow != null)
        {
            mapWindow.Close(delegate ()
            {
                Scenes.UnloadScene("Map");
                quitting = false;
            });
        }
        else
        {
            Scenes.UnloadScene("Map");
            quitting = false;
        }
    }
}
