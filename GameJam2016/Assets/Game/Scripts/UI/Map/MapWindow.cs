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
        Scenes.LoadAsync("Map", UnityEngine.SceneManagement.LoadSceneMode.Additive);
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
                Scenes.UnloadAsync("Map");
                quitting = false;
            });
        }
        else
        {
            Scenes.UnloadAsync("Map");
            quitting = false;
        }
    }
}
