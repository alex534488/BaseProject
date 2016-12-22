using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeManager : MonoBehaviour {

    static ModeManager modeManager;

    public List<Mode> modes = new List<Mode>();

    private Mode currentMode;

    void Awake()
    {
        if (modeManager == null) modeManager = this;
        currentMode = null;
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

