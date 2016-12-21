using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[CreateAssetMenu(menuName = "Request/Request Bank")]
public class RequestBank : ScriptableObject
{
    [System.Serializable]
    public class Item
    {
        public RequestFrame frame = null;
        public int weight = 1;
    }

    public List<Item> requests = new List<Item>();

    public RequestFrame GetFrame(string tag)
    {
        List<Item> matches = new List<Item>();
        foreach (Item item in requests)
        {
            string frameTag = item.frame.tag;

            if (frameTag.Length > tag.Length)                   //Si le tag est plus long que celui demandé (ex: village_random_gold > village_random)
                frameTag = frameTag.Substring(0, tag.Length);   //on va le couper pour seulement tenir compte de la première partie (village_random)

            if (frameTag == tag) matches.Add(item);
        }

        if (matches.Count == 0) return null;
        if (matches.Count == 1) return matches[0].frame;

        //Une pige au sort !
        int totalWeigth = 0;
        foreach (Item item in requests)
        {
            totalWeigth += item.weight;
        }
        int ticket = Random.Range(0, totalWeigth);
        int checkedTickets = 0;
        foreach (Item item in requests)
        {
            checkedTickets += item.weight;
            if (ticket < checkedTickets) return item.frame;
        }

        return matches[Random.Range(0, matches.Count)].frame; // Ne devrais pas arriver ici. Normalement, la request est choisie dans la boucle précédente
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RequestBank))]
public class RequestBankEditor : Editor
{
    ReorderableList list;

    void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("requests"), true, true, true, true);
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var item = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight),
                    item.FindPropertyRelative("frame"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    item.FindPropertyRelative("weight"), GUIContent.none);
            };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Requests (with weigth)");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif