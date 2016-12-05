using UnityEngine;
using System.Collections;
using CCC.UI;
using CCC.Utility;
using UnityEngine.Events;


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
            SetState(index, true);
        }
        public void SetAlive(int index)
        {
            SetState(index, false);
        }

        void SetState(int index, bool state)
        {
            if (index >= cityDestructions.Length || index < 0) return;

            cityDestructions[index] = state;
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

    public WindowAnimation windowAnimation;

    MapCity currentlySelected = null;

    void Awake()
    {
        Load(OnLoadComplete);
    }

    void OnLoadComplete()
    {
        MapLens.onSelect.AddListener(OnLensSelected);

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
                //Si le village n'est pas nul, mais la sauveguarde dit qu'il l'est, changer la sauveguarde
                if (mapSave.WasDestroyed(i))
                {
                    mapSave.SetAlive(i);
                    needToSave = true;
                }
                city.Init(village);
                city.onClick.AddListener(OnMapCitySelected);
            }
        }

        if (needToSave) Save();

        
        if (panel != null)
        {
            panel.onRequest.AddListener(Request);
            panel.onSend.AddListener(Send);
            panel.onClose.AddListener(OnPanelClose);
        }

        windowAnimation.Open();
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

    void Load(UnityAction onComplete = null)
    {
        string path = GameSave.GetFilePath() + "mapDestruction.dat";

        if (ThreadSave.Exists(path))
        {
            ThreadSave.Load(path,
                delegate (object obj)
                {
                    mapSave = (MapSave)obj;
                    if (onComplete != null)
                        onComplete();
                });
        }
        else
        {
            mapSave = new MapSave(cities.Length);
            Save(onComplete);
        }
    }

    void Save(UnityAction onComplete = null)
    {
        if (mapSave == null)
        {
            if(onComplete != null)onComplete();
            return;
        }

        string path = GameSave.GetFilePath() + "mapDestruction.dat";

        ThreadSave.Save(path,mapSave,
            delegate ()
            {
                if (onComplete != null)
                    onComplete();
            });
    }
}
