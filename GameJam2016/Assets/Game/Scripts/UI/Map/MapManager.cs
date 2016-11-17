using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
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

	void Awake ()
    {
        MapLens.onSelect.AddListener(OnLensSelected);

        Load();

        bool needToSave = false;

        //Init les village
        for(int i=0; i<cities.Length; i++)
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
            }
        }

        if (needToSave) Save();
    }

    void OnLensSelected(Resource_Type type)
    {
        foreach(MapCity city in cities)
        {
            city.Display(type);
        }
    }

    void OnDestroy()
    {
        MapLens.onSelect.RemoveAllListeners();
    }

    void Load()
    {
        //Temporaire
        mapSave = new MapSave(cities.Length);
        //TO DO
    }

    void Save()
    {
        //TO DO
    }
}
