using UnityEngine;
using System.Collections;

public class MapLensSave : MonoBehaviour {

    public MapLens[] mapLenses = new MapLens[0];
    
	void Start () {
        int lastOpened = 0;
        if (PlayerPrefs.HasKey("MapLens")) lastOpened = PlayerPrefs.GetInt("MapLens");

        try
        {
            mapLenses[lastOpened].Select();
        }
        catch
        {
            Debug.LogError("Error: Failed to select the last selected map lens.");
        }
	}

    void OnDestroy()
    {
        int i = 0;
        foreach(MapLens lens in mapLenses)
        {
            if (lens.IsSelected()) break;
            i++;
        }
        PlayerPrefs.SetInt("MapLens", i);
        PlayerPrefs.Save();
    }
}
