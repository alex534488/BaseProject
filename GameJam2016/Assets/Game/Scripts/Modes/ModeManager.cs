using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeManager : MonoBehaviour {

    public static ModeManager modeManager;

    public List<Mode> modes = new List<Mode>();

    public Mode currentMode = null;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        if (modeManager == null) modeManager = this;
    }

    // Changer le mode
    public void SetCurrentMode(string name)
    {
        for(int i = 0; i < modes.Count; i++)
        {
            if(modes[i].nom == name)
            {
                currentMode = modes[i];
            }
        }
    }

    public Mode GetCurrentMode()
    {
        return currentMode;
    }
}

