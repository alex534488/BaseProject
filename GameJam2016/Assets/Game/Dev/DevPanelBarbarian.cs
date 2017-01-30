using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DevPanelBarbarian : MonoBehaviour
{
    public Button mainButton;
    public GameObject buttonPrefab;
    public GameObject barbarelistCountainer;
    public GameObject spawnButton;
    private BarbareManager barbareManager;
    private World world;
    Color highlightColor = new Color(197.0f / 255.0f, 255.0f / 255.0f, 187.0f / 255.0f);

    public void Show(World world)
    {
        mainButton.image.color = highlightColor;
        gameObject.SetActive(true);
        this.world = world;
        UpdateDisplay(world);
    }
    public void Hide()
    {
        mainButton.image.color = Color.white;
        gameObject.SetActive(false);
        Clear();
    }

    public void UpdateDisplay(World world)
    {
        barbareManager = world.barbareManager;
        List<BarbarianClan> listBarbares = barbareManager.GetAllClans();

        Clear();
        for (int i = 0; i < listBarbares.Count; i++)
        {
            GameObject newBarbareButton = Instantiate(buttonPrefab);
            newBarbareButton.transform.GetComponentInChildren<Text>().text = "Barbare Clan " + i + " | Position : " + listBarbares[i].GetPosition() + " | ArmyPower: " + listBarbares[i].GetPower() + " | AttackCoolDown : " + listBarbares[i].GetCoolDown() + " | CoolDownCounter: " + listBarbares[i].GetCounter();
            newBarbareButton.transform.SetParent(barbarelistCountainer.transform);
            newBarbareButton.GetComponent<DevPanelBarbareClanInteract>().currentClan = listBarbares[i];
            newBarbareButton.GetComponent<DevPanelBarbareClanInteract>().currentWorld = world;
            newBarbareButton.GetComponent<DevPanelBarbareClanInteract>().devPanelBarbarian = this;
        }
    }

    public void OnClickSpawnBarbarianClan()
    {
        barbareManager.Spawn(1, null);
        UpdateDisplay(world);
    }

    private void Clear()
    {
        foreach (Transform child in barbarelistCountainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
