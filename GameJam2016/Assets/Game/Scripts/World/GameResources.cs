using UnityEngine;
using System.Collections;

public enum Ressource_Type
{
    gold, goldProd, food, foodProd, foodBilan, army, armyProd, happiness, happinessCap, reputation
}

public class GameResources
{
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
