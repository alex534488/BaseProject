using UnityEngine;
using System.Collections;

namespace Game.Characters
{
    [System.Serializable]
    public class Kit : IKit
    {
        public Sprite head;
        public Sprite body;
        public Sprite arms;
        public Sprite hair;
        public Color skinColor;
        public Color bodyColor;
        public Color hairColor;

        public Sprite Head()
        {
            return head;
        }

        public Sprite Body()
        {
            return body;
        }

        public Sprite Arms()
        {
            return arms;
        }

        public Sprite Hair()
        {
            return hair;
        }

        public Color SkinColor()
        {
            return skinColor;
        }

        public Color HairColor()
        {
            return hairColor;
        }

        public Color BodyColor()
        {
            return bodyColor;
        }
    }
}
