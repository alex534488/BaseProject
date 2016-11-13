using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CCC.Utility
{
    public class FPSCounter : SlowBehaviour
    {
        static FPSCounter instance;
        public Text display;
        void Awake()
        {
            instance = this;
        }

        protected override void SlowUpdate()
        {
            base.SlowUpdate();
            if (display != null)
            {
                int fps = (int)GetFPS();

                display.text = fps.ToString();
            }
        }

        public static float GetFPS()
        {
            return 1f / Time.deltaTime;
        }
    }
}
