using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Building/Building")]
public class Building : ScriptableObject
{
    // Nom
    [Tooltip("Nom du batiment")]
    public string nom = "default";

    // Description
    [Tooltip("Description du batiment")]
    public string description = "utilite du batiment ici";

    // Icone
    [Tooltip("Image correspondant au building")]
    public Sprite icon;

    // Batiment de village?
    [Tooltip("Le batiment peut etre construit dans les villages")]
    public bool IsAvailableInVillage = false;

    // Batiment pour la capitale?
    [Tooltip("Le batiment peut etre construit dans la capitale")]
    public bool IsAvailableInCapital = false;

    // Liste de batiment requis pour faire celui-ci
    [Tooltip("Liste de batiment requis pour faire celui-ci")]
    public List<Building> listRequirements;

    [HideInInspector]
    public string behaviorType;

    // Modificateur de Stats
    public List<VillageModifier> villageModifierList;
    public List<EmpireModifier> empireModifierList;

    // Obtenir le nom du batiment
    public string GetName()
    {
        return nom;
    }

    // Applique les modifications necessaire lors de l'achat du batiment
    public void Apply(Village village)
    {
        for (int i = 0; i < villageModifierList.Count; i++)
        {
            villageModifierList[i].ApplyModifier(village);
        }
        for (int i = 0; i < empireModifierList.Count; i++)
        {
            empireModifierList[i].ApplyModifier(Universe.Empire);
        }
    }

    public bool HasBehavior()
    {
        if (behaviorType == null || behaviorType == "" || Type.GetType(behaviorType, false) == null)
            return false;
        return true;
    }

    public BuildingBehavior CreateBehavior()
    {
        if (behaviorType == null || behaviorType == "")
            return null;

        System.Type type = Type.GetType(behaviorType, false);
        if (type == null)
        {
            Debug.LogWarning("Attempting to create BuildingBehavior with wrong type name.");
            return null;
        }

        BuildingBehavior behavior = (BuildingBehavior) Activator.CreateInstance(type);

        return behavior;
    }
}
//
#if UNITY_EDITOR
[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Building building = target as Building;

        EditorGUILayout.Space();

        //      Building Behavior : Marde                 [x]
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Building Behavior : " + (building.behaviorType != null ? building.behaviorType.ToString() : ""));
        if (GUILayout.Button("x", GUILayout.Width(20)))
        {
            building.behaviorType = null;
            EditorUtility.SetDirty(building);
        }
        EditorGUILayout.EndHorizontal();

        // Assignation du script
        MonoScript script = null;
        script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
        if (script != null && script.GetClass().IsSubclassOf(typeof(BuildingBehavior)))
        {
            building.behaviorType = script.GetClass().AssemblyQualifiedName;
            EditorUtility.SetDirty(building);
        }
    }
}
#endif