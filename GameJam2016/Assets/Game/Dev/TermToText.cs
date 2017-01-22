using UnityEngine;
using System.Collections;

public class TermToText
{

    static public string Term(Village village, bool colored = false)
    {
        return village != null ? village.Name : NullTerm(colored);
    }

    static public string NullTerm(bool colored = false)
    {
        return colored ? "<color=#FF0000FF>[null]</color>" : "[null]";
    }

    static public string Term(ResourceType type, bool colored = false)
    {
        if (GameResources.IsTypeEmpire(type))
            return Term(GameResources.Convert_ToEmpire(type), colored);
        else
            return Term(GameResources.Convert_ToVillage(type), colored);
    }
    static public string Term(Empire_ResourceType type, bool colored = false)
    {
        string baseText = GameResources.ToString(GameResources.Convert(type));
        if (!colored)
            return baseText;
        switch (type)
        {
            case Empire_ResourceType.science:
                return "<color=#006DA3FF>" + baseText + "</color>";
            case Empire_ResourceType.gold:
                return "<color=#A1A300FF>" + baseText + "</color>";
            case Empire_ResourceType.material:
                return "<color=#A36800FF>" + baseText + "</color>";
            case Empire_ResourceType.citizenProgress:
                return "<color=#05A300FF>" + baseText + "</color>";
            case Empire_ResourceType.happiness:
                return "<color=#56A300FF>" + baseText + "</color>";
            case Empire_ResourceType.reputation:
                return "<color=#A30098FF>" + baseText + "</color>";
            case Empire_ResourceType.citizenProgressMax:
                return "<color=#05A300FF>" + baseText + "</color>";
            default:
            case Empire_ResourceType.custom:
                return "<color=#FF00F7FF>" + baseText + "</color>";
        }
    }
    static public string Term(Village_ResourceType type, bool colored = false)
    {
        string baseText = GameResources.ToString(GameResources.Convert(type));
        if (!colored)
            return baseText;
        switch (type)
        {
            case Village_ResourceType.scienceProd:
                return "<color=#006DA3FF>" + baseText + "</color>";
            case Village_ResourceType.goldProd:
                return "<color=#A1A300FF>" + baseText + "</color>";
            case Village_ResourceType.materialProd:
                return "<color=#A36800FF>" + baseText + "</color>";
            case Village_ResourceType.food:
                return "<color=#05A300FF>" + baseText + "</color>";
            case Village_ResourceType.armyPower:
                return "<color=#A30000FF>" + baseText + "</color>";
            case Village_ResourceType.armyCost:
                return "<color=#A30000FF>" + baseText + "</color>";
            default:
            case Village_ResourceType.custom:
                return "<color=#FF00F7FF>" + baseText + "</color>";
        }
    }

    static public string Term(Transaction transaction, bool colored = false)
    {
        string text = "";
        text += transaction.value;
        switch (transaction.valueType)
        {
            default:
            case Transaction.ValueType.flat:
                break;
            case Transaction.ValueType.sourcePercent:
                text += " %s";
                break;
            case Transaction.ValueType.destPercent:
                text += " %d";
                break;
        }

        text += " " + Term(transaction.type, colored);
        text += " from " + Term(transaction.source, colored) + " to " + Term(transaction.destination, colored);


        return text;
    }
}
