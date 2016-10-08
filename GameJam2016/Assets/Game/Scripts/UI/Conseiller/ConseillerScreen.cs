using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ConseillerScreen : MonoBehaviour {

    public ConseillerScreenItem itemPrefab;
    public RectTransform container;
    public static string SCENE = "ConseillerPanel";
    List<Village> villages;
    Ressource_Type type;

    List<ConseillerScreenItem> items;

    public void Init(List<Village> villages, Ressource_Type type)
    {
        this.villages = villages;
        this.type = type;
        foreach(Village ville in villages)
        {
            ConseillerScreenItem item = Instantiate(itemPrefab.gameObject).GetComponent<ConseillerScreenItem>();
            item.transform.SetParent(container);
            items.Add(item);
        }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        int i = 0;
        foreach(ConseillerScreenItem item in items)
        {
            item.Display(villages[i], type);
            i++;
        }
    }

    public void Exit()
    {
        SceneManager.UnloadScene(SCENE);
    }
}