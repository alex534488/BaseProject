using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class MapLens : MonoBehaviour
{

    public Village_ResourceType type;
    public Image image;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.white;
    public static GameResources.Village_ResourceEvent onSelect = new GameResources.Village_ResourceEvent();

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

    static public Village_ResourceType CurrentType()
    {
        if (currentLens != null)
            return currentLens.type;
        else
            return Village_ResourceType.custom;
    }
}
