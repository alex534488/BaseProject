using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    // Nom
    [Tooltip("Nom de l'item")]
    public string nom = "default";

    // Description
    [Tooltip("Description de l'item")]
    public string description = "utilite de l'item ici";

    // Modificateur de Stats
    public List<VillageModifier> villageModifierList;
    public List<EmpireModifier> empireModifierList;

    [HideInInspector]
    public string behaviorType;

    // Obtenir le nom du batiment
    public string GetName()
    {
        return nom;
    }

    // Applique les modifications necessaire lors de l'achat de l'item
    public void Apply(Empire empire)
    {
        // apply the effect of this item on the empire
    }

    public bool HasBehavior()
    {
        if (behaviorType == null || behaviorType == "" || Type.GetType(behaviorType, false) == null)
            return false;
        return true;
    }

    public ItemBehavior CreateBehavior()
    {
        if (behaviorType == null || behaviorType == "")
            return null;

        System.Type type = Type.GetType(behaviorType, false);
        if (type == null)
        {
            Debug.LogWarning("Attempting to create ItemBehavior with wrong type name.");
            return null;
        }

        ItemBehavior behavior = (ItemBehavior)Activator.CreateInstance(type);

        return behavior;
    }
}

//
#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Item item = target as Item;

        EditorGUILayout.Space();

        //      Building Behavior : Marde                 [x]
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Behavior : " + (item.behaviorType != null ? item.behaviorType.ToString() : ""));
        if (GUILayout.Button("x", GUILayout.Width(20)))
        {
            item.behaviorType = null;
            EditorUtility.SetDirty(item);
        }
        EditorGUILayout.EndHorizontal();

        // Assignation du script
        MonoScript script = null;
        script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
        if (script != null && script.GetClass().IsSubclassOf(typeof(BuildingBehavior)))
        {
            item.behaviorType = script.GetClass().AssemblyQualifiedName;
            EditorUtility.SetDirty(item);
        }
    }
}
#endif
