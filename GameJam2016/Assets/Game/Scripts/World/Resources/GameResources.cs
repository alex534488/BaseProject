﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public enum ResourceType
{
    science = 1,
    scienceProd = 2,
    gold = 3,
    goldProd = 4,
    material = 5,
    materialProd = 6,
    citizenProgress = 7,
    food = 8,
    happiness = 9,
    reputation = 10,
    armyPower = 11,
    armyCost = 12,
    armyHitRate = 13,
    custom = 14
}

public enum Village_ResourceType
{
    scienceProd = 1,
    goldProd = 2,
    materialProd = 3,
    food = 4,
    armyPower = 5,
    custom = 6
}

public enum Empire_ResourceType
{
    science = 1,
    gold = 2,
    material = 3,
    citizenProgress = 4,
    armyCost = 5,
    armyHitRate = 6,
    happiness = 7,
    reputation = 8,
    custom = 9
}

public class GameResources : MonoBehaviour
{
    public class ResourceEvent : UnityEvent<ResourceType> { }
    public class Village_ResourceEvent : UnityEvent<Village_ResourceType> { }
    public class Empire_ResourceEvent : UnityEvent<Empire_ResourceType> { }
    public Sprite scienceIcon;
    public Sprite scienceProdIcon;
    public Sprite goldIcon;
    public Sprite goldProdIcon;
    public Sprite materialIcon;
    public Sprite materialProdIcon;
    public Sprite citizenProgressIcon;
    public Sprite foodIcon;
    public Sprite happinessIcon;
    public Sprite reputationIcon;
    public Sprite armyPowerIcon;
    public Sprite armyCostIcon;
    public Sprite armyHitRate;
    public Sprite citizenProgressMax;

    static GameResources instance;

    void Awake()
    {
        instance = this;
    }

    public static Sprite GetIcon(ResourceType type)
    {
        if (instance == null) { Debug.LogError("Error: instance is null."); return null; }
        switch (type)
        {
            default:
            case ResourceType.science:
                return instance.scienceIcon;
            case ResourceType.scienceProd:
                return instance.scienceProdIcon;
            case ResourceType.gold:
                return instance.goldIcon;
            case ResourceType.goldProd:
                return instance.goldProdIcon;
            case ResourceType.material:
                return instance.materialIcon;
            case ResourceType.materialProd:
                return instance.materialProdIcon;
            case ResourceType.citizenProgress:
                return instance.citizenProgressIcon;
            case ResourceType.food:
                return instance.foodIcon;
            case ResourceType.happiness:
                return instance.happinessIcon;
            case ResourceType.reputation:
                return instance.reputationIcon;
            case ResourceType.armyPower:
                return instance.armyPowerIcon;
            case ResourceType.armyCost:
                return instance.armyCostIcon;
            case ResourceType.armyHitRate:
                return instance.armyHitRate;
        }
    }

    public static string ToString(ResourceType stdType)
    {
        switch (stdType)
        {
            default: return stdType.ToString();
            case ResourceType.science:
                return "Science";
            case ResourceType.scienceProd:
                return "Science Production";
            case ResourceType.gold:
                return "Gold";
            case ResourceType.goldProd:
                return "Gold Production";
            case ResourceType.material:
                return "Material";
            case ResourceType.materialProd:
                return "Material Production";
            case ResourceType.citizenProgress:
                return "Citizen Progress";
            case ResourceType.food:
                return "Food";
            case ResourceType.happiness:
                return "Happiness";
            case ResourceType.reputation:
                return "Reputation";
            case ResourceType.armyPower:
                return "Army Power";
            case ResourceType.armyCost:
                return "Army Cost";
            case ResourceType.armyHitRate:
                return "Army HitRate";
        }
    }

    public static ResourceType Convert(Village_ResourceType type)
    {
        switch (type)
        {
            case Village_ResourceType.scienceProd:
                return ResourceType.scienceProd;
            case Village_ResourceType.goldProd:
                return ResourceType.goldProd;
            case Village_ResourceType.materialProd:
                return ResourceType.materialProd;
            case Village_ResourceType.food:
                return ResourceType.food;
            case Village_ResourceType.armyPower:
                return ResourceType.armyPower;
            default:
                Debug.LogWarning("Cannot convert " + type + " to a ResourceType");
                return ResourceType.custom;
        }
    }

    public static ResourceType Convert(Empire_ResourceType type)
    {
        switch (type)
        {
            case Empire_ResourceType.science:
                return ResourceType.science;
            case Empire_ResourceType.gold:
                return ResourceType.goldProd;
            case Empire_ResourceType.material:
                return ResourceType.material;
            case Empire_ResourceType.citizenProgress:
                return ResourceType.citizenProgress;
            case Empire_ResourceType.happiness:
                return ResourceType.happiness;
            case Empire_ResourceType.reputation:
                return ResourceType.reputation;
            case Empire_ResourceType.armyCost:
                return ResourceType.armyCost;
            case Empire_ResourceType.armyHitRate:
                return ResourceType.armyHitRate;
            default:
                Debug.LogWarning("Cannot convert " + type + " to a ResourceType");
                return ResourceType.custom;
        }
    }

    public static Village_ResourceType Convert_ToVillage(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.scienceProd:
                return Village_ResourceType.scienceProd;
            case ResourceType.goldProd:
                return Village_ResourceType.goldProd;
            case ResourceType.materialProd:
                return Village_ResourceType.materialProd;
            case ResourceType.food:
                return Village_ResourceType.food;
            case ResourceType.armyPower:
                return Village_ResourceType.armyPower;
            default:
                Debug.LogWarning("Cannot convert " + type + " to a Village_ResourceType");
                return Village_ResourceType.custom;
        }
    }

    public static Empire_ResourceType Convert_ToEmpire(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.science:
                return Empire_ResourceType.science;
            case ResourceType.gold:
                return Empire_ResourceType.gold;
            case ResourceType.material:
                return Empire_ResourceType.material;
            case ResourceType.citizenProgress:
                return Empire_ResourceType.citizenProgress;
            case ResourceType.happiness:
                return Empire_ResourceType.happiness;
            case ResourceType.reputation:
                return Empire_ResourceType.reputation;
            case ResourceType.armyCost:
                return Empire_ResourceType.armyCost;
            case ResourceType.armyHitRate:
                return Empire_ResourceType.armyHitRate;
            default:
                Debug.LogWarning("Cannot convert " + type + " to a Empire_ResourceType");
                return Empire_ResourceType.custom;
        }
    }

    public static bool IsTypeEmpire(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.science:
                return true;
            case ResourceType.scienceProd:
                return false;
            case ResourceType.gold:
                return true;
            case ResourceType.goldProd:
                return false;
            case ResourceType.material:
                return true;
            case ResourceType.materialProd:
                return false;
            case ResourceType.citizenProgress:
                return true;
            case ResourceType.food:
                return false;
            case ResourceType.happiness:
                return true;
            case ResourceType.reputation:
                return true;
            case ResourceType.armyPower:
                return false;
            case ResourceType.armyCost:
                return true;
            case ResourceType.armyHitRate:
                return true;
            default:
                return false;
        }
    }

    public static bool IsTypeVillage(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.science:
                return false;
            case ResourceType.scienceProd:
                return true;
            case ResourceType.gold:
                return false;
            case ResourceType.goldProd:
                return true;
            case ResourceType.material:
                return false;
            case ResourceType.materialProd:
                return true;
            case ResourceType.citizenProgress:
                return false;
            case ResourceType.food:
                return true;
            case ResourceType.happiness:
                return false;
            case ResourceType.reputation:
                return false;
            case ResourceType.armyPower:
                return true;
            case ResourceType.armyCost:
                return false;
            case ResourceType.armyHitRate:
                return false;
            default:
                return false;
        }
    }

    static public Color GetBrightColor(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.science:
                return new Color(0, 0.5f, 1);
            case ResourceType.scienceProd:
                return new Color(0, 0.5f, 1);
            case ResourceType.gold:
                return new Color(1, 1, 0);
            case ResourceType.goldProd:
                return new Color(1, 1, 0);
            case ResourceType.material:
                return new Color(0.7f, 0.4f, 0.06f);
            case ResourceType.materialProd:
                return new Color(0.7f, 0.4f, 0.06f);
            case ResourceType.citizenProgress:
                return new Color(0.5f, 1, 0);
            case ResourceType.food:
                return new Color(0.5f, 1, 0);
            case ResourceType.happiness:
                return new Color(0, 1, 0.25f);
            case ResourceType.reputation:
                return new Color(1, 0, 1);
            case ResourceType.armyPower:
                return new Color(1, 0, 0);
            case ResourceType.armyCost:
                return new Color(1, 0, 0);
            case ResourceType.armyHitRate:
                return new Color(1, 0, 0);
            default:
            case ResourceType.custom:
                return new Color(1, 1, 1);
        }
    }
}
