using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using CCC.Manager;

public class DevPanelBarbareMenu : MonoBehaviour {

    public InputField inputField;
    public BarbarianClan currentClan;
    public DevPanelBarbarian devPanelBarbarian;

    public void SetCurrentClan(BarbarianClan currentClan)
    {
        this.currentClan = currentClan;
    }

    public void OnClickMoveTo()
    {
        if(inputField.text != null)
        {
            currentClan.OnMoving(int.Parse(inputField.text));
            devPanelBarbarian.UpdateDisplay();
        }
    }

    public void OnClickSpawn()
    {
        if (inputField.text != null)
        {
            currentClan.AddPower(int.Parse(inputField.text));
            devPanelBarbarian.UpdateDisplay();
        }
    }

    public void OnClickNewDay()
    {
        if (inputField.text != null)
        {
            for (int i = 0; i < int.Parse(inputField.text); i++)
            {
                currentClan.NewDay();
            }
            devPanelBarbarian.UpdateDisplay();
        }
    }

    public void OnDelete()
    {
        Universe.Barbares.Delete(currentClan);
        devPanelBarbarian.UpdateDisplay();
        Scenes.UnloadScene("InteractBarbare");
    }

    public void Leave()
    {
        Scenes.UnloadScene("InteractBarbare");
    }
}
