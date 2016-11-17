using UnityEngine;
using System.Collections;

[System.Serializable]
public enum Ressource_Type
{
    gold, goldProd, food, foodProd, army, armyProd, happiness, happinessCap, reputation
}

public class GameResources : MonoBehaviour
{
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

    public static Sprite GetIcon(Ressource_Type type)
    {
        if(instance == null) { Debug.LogError("Error: instance is null."); return null; }
        switch (type)
        {
            default:
            case Ressource_Type.army:
                return instance.armyIcon;
            case Ressource_Type.armyProd:
                return instance.armyProdIcon;
            case Ressource_Type.food:
                return instance.foodIcon;
            case Ressource_Type.foodProd:
                return instance.foodProdIcon;
            case Ressource_Type.gold:
                return instance.goldIcon;
            case Ressource_Type.goldProd:
                return instance.goldProdIcon;
            case Ressource_Type.happiness:
                return instance.happinessIcon;
            case Ressource_Type.happinessCap:
                return instance.happinessCapIcon;
            case Ressource_Type.reputation:
                return instance.reputationIcon;
        }
    }

    public static Ressource_Type GetAlternate(Ressource_Type stdType)
    {
        switch (stdType)
        {
            default: return Ressource_Type.goldProd;
            case Ressource_Type.army:
                return Ressource_Type.armyProd;
            case Ressource_Type.food:
                return Ressource_Type.foodProd;
            case Ressource_Type.gold:
                return Ressource_Type.goldProd;
            case Ressource_Type.happiness:
                return Ressource_Type.happinessCap;
        }
    }

    public static string ToString(Ressource_Type stdType)
    {
        switch (stdType)
        {
            default: return stdType.ToString();
            case Ressource_Type.army:
                return "armé";
            case Ressource_Type.food:
                return "nourriture";
            case Ressource_Type.gold:
                return "or";
            case Ressource_Type.happiness:
                return "bonheur";
            case Ressource_Type.reputation:
                return "réputation";
        }
    }
}
