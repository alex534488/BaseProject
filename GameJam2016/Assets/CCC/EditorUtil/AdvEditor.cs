using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CCC.EditorUtil
{
    public class AdvEditor : Editor
    {
        protected GUIStyle bold = new GUIStyle();
        protected GUIStyle centered = new GUIStyle();
        protected GUIStyle righted = new GUIStyle();


        void Awake()
        {
            bold.fontStyle = FontStyle.Bold;
            centered.alignment = TextAnchor.MiddleCenter;
            righted.alignment = TextAnchor.MiddleRight;
        }
    }
}
#endif