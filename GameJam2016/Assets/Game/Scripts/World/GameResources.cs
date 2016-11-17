using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public enum Resource_Type
{
    gold, goldProd, food, foodProd, army, armyProd, happiness, happinessCap, reputation, custom
}

public class GameResources : MonoBehaviour
{
    public class ResourceEvent:UnityEvent<Resource_Type>{ }
    public Sprite goldIcon;
    public Sprite goldProdIcon;
    public Sprite foodIcon;
    public Sprite foodProdIcon;
    public Sprite armyIcon;
    public Sprite armyProdIcon;
    public Sprite reputationIcon;
    public Sprite happinessIcon;
    public Sprite happinessCapIcon;

    static GameResources instance;

    void Awake()
    {
        instance = this;
    }

    public static Sprite GetIcon(Resource_Type type)
    {
        if(instance == null) { Debug.LogError("Error: instance is null."); return null; }
        switch (type)
        {
            default:
            case Resource_Type.army:
                return instance.armyIcon;
            case Resource_Type.armyProd:
                return instance.armyProdIcon;
            case Resource_Type.food:
                return instance.foodIcon;
            case Resource_Type.foodProd:
                return instance.foodProdIcon;
            case Resource_Type.gold:
                return instance.goldIcon;
            case Resource_Type.goldProd:
                return instance.goldProdIcon;
            case Resource_Type.happiness:
                return instance.happinessIcon;
            case Resource_Type.happinessCap:
                return instance.happinessCapIcon;
            case Resource_Type.reputation:
                return instance.reputationIcon;
        }
    }

    public static Resource_Type GetAlternate(Resource_Type stdType)
    {
        switch (stdType)
        {
            default: return Resource_Type.goldProd;
            case Resource_Type.army:
                return Resource_Type.armyProd;
            case Resource_Type.food:
                return Resource_Type.foodProd;
            case Resource_Type.gold:
                return Resource_Type.goldProd;
            case Resource_Type.happiness:
                return Resource_Type.happinessCap;
        }
    }

    public static string ToString(Resource_Type stdType)
    {
        switch (stdType)
        {
            default: return stdType.ToString();
            case Resource_Type.army:
                return "armé";
            case Resource_Type.food:
                return "nourriture";
            case Resource_Type.gold:
                return "or";
            case Resource_Type.happiness:
                return "bonheur";
            case Resource_Type.reputation:
                return "réputation";
        }
    }
}
