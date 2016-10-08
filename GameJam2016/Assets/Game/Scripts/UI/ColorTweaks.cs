using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LuckyHammers.UI
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [ExecuteInEditMode]
    public class ColorTweaks : BaseGraphicEffect
    {
        public const string SHADER = "LuckyHammers/UI/ColorTweaks";

        static public ColorTweaks CheckIn(Graphic target)
        {
            ColorTweaks colorTweaks = target.GetComponent<ColorTweaks>();
            if (colorTweaks == null) colorTweaks = target.gameObject.AddComponent<ColorTweaks>();
            return colorTweaks;
        }

        [Header("Additive Color"), Range(0, 1), Tooltip("Additive color amount")]
        public float add = 0;
        public Color addColor = new Color(1, 1, 1, 1);

        [Header("Color Controls"), Range(0, 4)]
        public float brightness = 1;

        [Range(0, 2)]
        public float saturation = 1;

        [Tooltip("Saturation effet on brightness: left = no saturation, right normal saturation")]
        public AnimationCurve saturationBrightness = new AnimationCurve( new Keyframe[]
        {
            new Keyframe(0, 0.85f),
            new Keyframe(1, 1)
        });

        protected override void Awake()
        {
            if (shader == null) shader = Shader.Find(SHADER);

            base.Awake();
        }
        
        void Update()
        {
            if (material != null)
            {
                /// Color must be sent to shader each frame
                material.SetColor("_Color", graphic.color);
            }
        }

        public override void UpdateMaterial()
        {
            // When called before awake
            if (graphic == null) graphic = GetComponent<Graphic>();

            if (material == null) {
                base.UpdateMaterial();
                return;
            }

            material.SetColor("_Color", graphic.color);
            material.SetFloat("_Saturation", saturation);

            if (saturationBrightness.length > 0)
            {
                material.SetFloat("_Brightness", brightness * saturationBrightness.Evaluate(saturation));
            }
            else
            {
                material.SetFloat("_Brightness", brightness);
            }

            material.SetFloat("_Add", add);
            material.SetColor("_AddColor", addColor);

            base.UpdateMaterial();
        }

        /// <summary>
        /// If the graphic effect gives a neutral result and should be disabled to save draw calls
        /// </summary>
        public override bool IsNeutral()
        {
            if (saturation != 1) return false;
            if(brightness>1) return false;
            if (add > 0) return false;
            return true;
        }

        public void SetSaturation(float to)
        {
            saturation = to;
            UpdateMaterial();
            if (!enabled && to != 1) enabled = true;
        }

        public void SetAdd(float to)
        {
            add = to;
            UpdateMaterial();
            if (!enabled && add > 0) enabled = true;
        }

    }
}

#if UNITY_EDITOR
namespace LuckyHammers.UI
{
    using UnityEditor;
    using LuckyHammers;

    [CustomEditor(typeof(ColorTweaks), true)]
    public class ColorTweaksEditor : BaseGraphicEffect_Editor
    {
        public Color lastAddColor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColorTweaks target = this.target as ColorTweaks;

            if (target.addColor != lastAddColor)
            {
                lastAddColor = target.addColor;
                target.UpdateMaterial();
            }
        }
    }
}
#endif