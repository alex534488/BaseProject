using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LuckyHammers.UI
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class BaseGraphicEffect : MonoBehaviour
    {
        public bool autoDisable = true;

        public Shader shader;

        [System.NonSerialized]
        public Graphic graphic;

        [System.NonSerialized]
        public Material material;

        bool wasEnabled = true;

        protected virtual void Awake()
        {
            if (graphic == null) graphic = GetComponent<Graphic>();
            wasEnabled = enabled;
        }

        protected virtual void OnEnable()
        {
            if (material == null) CreateMaterial();
            UpdateMaterial();
            graphic.material = material;
            wasEnabled = true;
        }

        protected virtual void OnDisable()
        {
            if (graphic!=null && graphic.material == material) graphic.material = null;
        }

        protected virtual void CreateMaterial()
        {
            if (shader == null)
            {
                Debug.LogError("Please set shader");
                return;
            }
            if(material==null) material = new Material(shader);
            graphic.material = material;
        }

        /*protected virtual void Update()
        {
            // TODO: check to find when to change that
            UpdateMaterial();
        }*/

        public virtual void UpdateMaterial()
        {
            if (autoDisable && wasEnabled) enabled = !IsNeutral();

            #if UNITY_EDITOR
            if (enabled && shader!=null) CreateMaterial();
            #endif
        }


        /// <summary>
        /// If the graphic effect gives a neutral result and should be disabled to save draw calls
        /// </summary>
        public virtual bool IsNeutral()
        {
            return false;
        }

        public virtual void Reset()
        {
            if (graphic != null && graphic.material == material) graphic.material = null;
            material = null;
            CreateMaterial();
            UpdateMaterial();
        }

        protected virtual void OnDestroy()
        {
            if(graphic!=null && graphic.material==material) graphic.material = null;
            material = null;
            graphic = null;
        }
    }
}

#if UNITY_EDITOR
namespace LuckyHammers.UI
{
    using UnityEditor;
    using LuckyHammers;

    [CustomEditor(typeof(BaseGraphicEffect), true)]
    public class BaseGraphicEffect_Editor : Editor
    {
        public Color lastColor;
        public Shader lastShader;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BaseGraphicEffect target = this.target as BaseGraphicEffect;

            if (GUI.changed || target.GetComponent<Graphic>().color != lastColor || target.shader != lastShader) //  || target.GetComponent<Graphic>().material.name == "Default UI Material"
            {
                lastColor = target.GetComponent<Graphic>().color;
                lastShader = target.shader;
                if(target.material!=null) target.UpdateMaterial();
            }
        }
    }
    
    public class BaseGraphicEffectProcessor : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            BaseGraphicEffect effect;
            Object[] objs = Component.FindObjectsOfType(typeof(BaseGraphicEffect));
            foreach (Object obj in objs)
            {
                effect = obj as BaseGraphicEffect;

                // Prevent runtime material from saving into scene
                effect.GetComponent<Graphic>().material = null;
                EditorUtility.SetDirty(effect.GetComponent<Graphic>());
            }

            return paths;
        }
    }

}
#endif