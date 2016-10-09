using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RemainingCarriage : MonoBehaviour {

    public Text text;

    void Awake()
    {
        Empire.instance.capitale.onCharriotChange.AddListener(UpdateDisplay);
        UpdateDisplay(0);
    }

    void UpdateDisplay(int dummy)
    {
        text.text = "Restant: " + Empire.instance.capitale.nbCharriot;
    }
}
