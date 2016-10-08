using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConseillerScreenItem : MonoBehaviour {

    public GameObject statPrefab;
    public Text cityName;
    public RectTransform statContainer;
    public Button upButton;
    public Button downButton;

    Village village;
    Ressource_Type type;

    void Awake()
    {
        statPrefab.gameObject.SetActive(false);
        upButton.onClick.AddListener(OnUpClick);
        downButton.onClick.AddListener(OnDownClick);
    }

    public void Display(Village village, Ressource_Type type)
    {
        this.type = type;
        this.village = village;

        cityName.text = village.nom;

        for(int i=0; i<3; i++)
        {
            Text text = Instantiate(statPrefab.gameObject).GetComponentInChildren<Text>();
            //text.text = "" + stat;
            text.transform.SetParent(statContainer, false);
        }
        //foreach(int stat in village.stats)
        //{
        //    Text text = Instantiate(statPrefab.gameObject).GetComponent<Text>();
        //    text.text = "" + stat;
        //    text.transform.SetParent(statContainer, false);
        //}
    }

    void OnUpClick()
    {
        // ++ taxe
    }

    void OnDownClick()
    {
        // -- taxe
    }
}
