using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CCC.Manager;

public class ConseillerScreen : MonoBehaviour {

    public ConseillerScreenItem itemPrefab;
    public Text titre;
    public AudioClip clip;
    public RectTransform container;
    public static string SCENE = "ConseillerPanel";
    List<Village> villages;
    Ressource_Type type;

    List<ConseillerScreenItem> items = new List<ConseillerScreenItem>();

    bool hasInit =false;

    void Awake()
    {
        if(!hasInit)transform.GetChild(0).gameObject.SetActive(false);
    }


    public void Init(List<Village> villages, Ressource_Type type)
    {
        this.villages = villages;
        this.type = type;
        foreach(Village ville in villages)
        {
            ConseillerScreenItem item = (Instantiate(itemPrefab.gameObject, container) as GameObject).GetComponent<ConseillerScreenItem>();
            item.transform.localScale = Vector3.one;
            items.Add(item);
        }

        hasInit = true;
        transform.GetChild(0).gameObject.SetActive(true);

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        switch (type)
        {
            default:
            case Ressource_Type.food:
                titre.text = "Nourriture de l'empire";
                break;
            case Ressource_Type.gold:
                titre.text = "Or de l'empire";
                break;
            case Ressource_Type.army:
                titre.text = "Armé de l'empire";
                break;
            case Ressource_Type.happiness:
                titre.text = "Bonheur de l'empire";
                break;
        }
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