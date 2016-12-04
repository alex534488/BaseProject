using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class MapSave
    {
        bool[] cityDestructions;
        public MapSave(int count)
        {
            cityDestructions = new bool[count];
        }
        public void SetDestoyed(int index)
        {
            if (index >= cityDestructions.Length || index < 0) return;

            cityDestructions[index] = true;
        }

        public bool WasDestroyed(int index)
        {
            if (index >= cityDestructions.Length || index < 0) return false;
            return cityDestructions[index];
        }
    }

    public MapCity[] cities = new MapCity[0];
    public MapSave mapSave;
    public MapCityPanel panel = null;

    MapCity currentlySelected = null;

    void Awake()
    {
        MapLens.onSelect.AddListener(OnLensSelected);

        Load();

        bool needToSave = false;

        //Init les village
        for (int i = 0; i < cities.Length; i++)
        {
            MapCity city = cities[i];
            Village village = Empire.instance.GetVillageByName(city.cityName);

            //Si village = null -> detruit
            if (village == null)
            {
                //Si !WasDestroyed -> le joueur n'a jamais vue ce village mourir, on enchene l'animation !
                if (!mapSave.WasDestroyed(i))
                {
                    mapSave.SetDestoyed(i);
                    needToSave = true;
                    city.DestroyAnim();
                }
                else
                {
                    city.gameObject.SetActive(false);
                }
            }
            else
            {
                city.Init(village);
                city.onClick.AddListener(OnMapCitySelected);
            }
        }

        if (needToSave) Save();



        //Spawn panel if inexisting
        if (panel != null)
        {
            panel.onRequest.AddListener(Request);
            panel.onSend.AddListener(Send);
            panel.onClose.AddListener(OnPanelClose);
        }
        else
            Debug.LogError("CityPanel reference is null.");
    }

    void OnLensSelected(Resource_Type type)
    {
        foreach (MapCity city in cities)
        {
            city.Display(type, GameResources.GetAlternate(type));
        }
    }

    void OnMapCitySelected(MapCity city)
    {
        currentlySelected = city;
        currentlySelected.Highlight();

        bool buttonsEnabled = MapLens.CurrentType() != Resource_Type.reputation && MapLens.CurrentType() != Resource_Type.custom;

        panel.Open(city.cityText.rectTransform.position - (Vector3.up * Screen.height * 0.17f), buttonsEnabled);
    }

    void OnPanelClose()
    {
        if (currentlySelected != null)
            currentlySelected.StopHighlight();
    }

    void Send(int amount)
    {
        if (MapLens.CurrentType() == Resource_Type.custom) return;

        Empire.instance.capitale.SendCartToVillage(currentlySelected.GetVillage(), MapLens.CurrentType(), amount);
    }

    void Request()
    {
        if (MapLens.CurrentType() == Resource_Type.custom) return;

        Empire.instance.capitale.SendCartToVillage(currentlySelected.GetVillage(), MapLens.CurrentType(), -1);
    }

    void OnDestroy()
    {
        MapLens.onSelect.RemoveAllListeners();
    }

    void Load()
    {
        string path = GameSave.GetFilePath() + "mapDestruction";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            mapSave = (MapSave)bf.Deserialize(file);
            file.Close();
            Debug.Log("Load Map");
        }
        else
        {
            mapSave = new MapSave(cities.Length);
            Save();
        }
    }

    void Save()
    {
        if (mapSave == null)
            return;

        string path = GameSave.GetFilePath() + "mapDestruction";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        bf.Serialize(file, mapSave);
        file.Close();
        Debug.Log("Save Map");
    }
}
