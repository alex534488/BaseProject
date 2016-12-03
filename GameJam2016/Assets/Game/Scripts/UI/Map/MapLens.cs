using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class MapLens : MonoBehaviour
{

    public Resource_Type type;
    public Image image;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.white;
    public static GameResources.ResourceEvent onSelect = new GameResources.ResourceEvent();

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
        onSelect.Invoke(type);

        //Change color & stuff
        image.color = selectedColor;
    }

    public bool IsSelected()
    {
        return currentLens == this;
    }

    static public Resource_Type CurrentType()
    {
        if (currentLens != null)
            return currentLens.type;
        else
            return Resource_Type.custom;
    }
}
