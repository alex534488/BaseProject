using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RemainingCarriage : MonoBehaviour {

    public Text text;

    void Awake()
    {
        Universe.CartsManager.OnAvailableCartChange.AddListener(UpdateDisplay);
        UpdateDisplay(0);
    }

    void UpdateDisplay(int dummy)
    {
        text.text = "Remaining: " + Universe.CartsManager.AvailableCarts;
    }
}
