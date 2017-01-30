using UnityEngine;
using System.Collections;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class DevPanelBarbareClanInteract : MonoBehaviour
{
    public string sceneName;
    public BarbarianClan currentClan;
    public World currentWorld;
    public DevPanelBarbarian devPanelBarbarian;

    public void ShowInteractMenu()
    {
        Scenes.LoadAsync(sceneName, LoadSceneMode.Additive, SetCurrentClan);
    }

    void SetCurrentClan(UnityEngine.SceneManagement.Scene newScene)
    {
        GameObject content = GameObject.Find("Box");
        DevPanelBarbareMenu[] buttons = content.GetComponentsInChildren<DevPanelBarbareMenu>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (currentClan != null)
            {
                buttons[i].GetComponent<DevPanelBarbareMenu>().SetCurrentClan(currentClan, currentWorld);
                buttons[i].GetComponent<DevPanelBarbareMenu>().devPanelBarbarian = devPanelBarbarian;
            }
            else
            {
                if (buttons[i].name != "Spawn")
                {
                    buttons[i].transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
