using UnityEngine;
using System.Collections;
using CCC.Manager;

public class DevPanelDoor : MonoBehaviour {
#if UNITY_EDITOR

    void Start()
    {
        MasterManager.Sync();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Scenes.Exists("DevPanel"))
                Scenes.UnloadAsync("DevPanel");
            else
                Scenes.Load("DevPanel", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }
#endif
}
