using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapLens : MonoBehaviour {

    public Ressource_Type type;
    public Image image;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.white;

    static MapLens currentLens = null;

    public void Deselect()
    {
        //Reset color
        image.color = normalColor;
    }

    public void Select()
    {
        if (currentLens != null) currentLens.Deselect();
        currentLens = this;

        //Change color & stuff
        image.color = selectedColor;
    }

    public bool IsSelected()
    {
        return currentLens == this;
    }
}
