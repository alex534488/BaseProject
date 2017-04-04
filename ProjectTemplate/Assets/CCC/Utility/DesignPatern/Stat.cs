using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Runtime.Serialization;

namespace CCC.Utility
{
    // Cette librairie permet d'utiliser une structure de données enregistrant des types de
    // données identifiant une statistique dans le jeu et pouvant être manipulé de diverses facons


    public enum BoundMode
    {
        Cap, MaxLoop, MinLoop, BidirectionalLoop
    }
    public enum BuffType
    {
        Flat, Percent
    }
    [Serializable]
    public class Stat<T>
    {
        public class StatEvent : UnityEvent<T> { };

        T value;
        public BoundMode boundMode = BoundMode.Cap;
        IComparable max = null;
        IComparable min = null;
        public T MAX
        {
            get { return (T)max; }
            set { max = (IComparable)value; onMaxSet.Invoke((T)value); }
        }
        public T MIN
        {
            get { return (T)min; }
            set { min = (IComparable)value; onMinSet.Invoke((T)value); }
        }
        [NonSerialized]
        public StatEvent onSet = new StatEvent();
        [NonSerialized]
        public StatEvent onMinReached = new StatEvent();
        [NonSerialized]
        public StatEvent onMaxReached = new StatEvent();
        [NonSerialized]
        public StatEvent onMinSet = new StatEvent();
        [NonSerialized]
        public StatEvent onMaxSet = new StatEvent();

        [Serializable]
        class Buff
        {
            public T value;
            public BuffType type;
            public Buff (T value, BuffType type)
            {
                this.value = value;
                this.type = type;
            }
        }
        Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();

        public Stat(T value)
        {
            boundMode = BoundMode.Cap;
            Set(value);
        }

        public Stat(T value, IComparable min, IComparable max, BoundMode boundMode)
        {
            this.boundMode = boundMode;
            this.min = min;
            this.max = max;
            Set(value);
        }

        [OnDeserializing]
        private void OnLoad(StreamingContext context)
        {
            onSet = new StatEvent();
            onMinReached = new StatEvent();
            onMaxReached = new StatEvent();
            onMinSet = new StatEvent();
            onMaxSet = new StatEvent();
        }

        public Stat<T> Set(T value, bool applyBuffs = true)
        {
            if (applyBuffs)
            {
                T delta = Sub(this.value, value);
                Set(Add(this.value, ApplyAllBuffs(delta)), false);
                return this;
            }

            if (value is IComparable)                           // Can be checked
            {
                if (min != null && (min.CompareTo(value) > 0 || min.Equals(value)))            // Check min
                {
                    if (min.CompareTo(value) > 0)
                    {
                        if ((boundMode == BoundMode.MinLoop || boundMode == BoundMode.BidirectionalLoop) && max != null)
                            QuickSet(Sub(Sub(value, MIN), MAX)); // équivaut à MAX - (MIN - value)
                        else
                            QuickSet(MIN);
                    }
                    else
                        QuickSet(value);

                    onMinReached.Invoke(value);
                }
                else if (max != null && (max.CompareTo(value) < 0 || max.Equals(value)))       // Check max
                {
                    if (max.CompareTo(value) < 0)
                    {
                        if ((boundMode == BoundMode.MaxLoop || boundMode == BoundMode.BidirectionalLoop) && min != null)
                        {
                            T newVal0 = Sub(MAX, value);
                            T newVal = Add(newVal0, MIN);
                            QuickSet(newVal); // équivaut à MIN + (value - MAX)
                        }
                        else
                            QuickSet(MAX);
                    }
                    else
                        QuickSet(value);
                    onMaxReached.Invoke(value);
                }
                else QuickSet(value);
            }
            else                                                // Cannot be checked
            {
                QuickSet(value);
            }

            return this;
        }

        void QuickSet(T value)
        {
            this.value = value;
            onSet.Invoke(value);
        }

        /// <summary>
        /// Test if the 'set value' is within min/max range. Returns false if out-of-bounds
        /// </summary>
        public bool TestSet(IComparable value)
        {
            if (min != null && min.CompareTo(value) > 0) return false;
            else if (max != null && max.CompareTo(value) < 0) return false;
            return true;
        }

        public System.Type Type() { return typeof(T); }

        public void PrintTest()
        {
            Debug.Log("value: " + value);
            foreach (KeyValuePair<string,Buff> buff in buffs)
            {
                Debug.Log("id: " + buff.Key + "  effect: " + buff.Value.value + " / " + buff.Value.type);
            }
        }

        /// <summary>
        /// Add buff that can later be removed. For 'percent' types, use whole numbers. ex: 35 -> +35%
        /// </summary>
        public bool AddBuff(string id, T value, BuffType type = BuffType.Percent)
        {
            if (buffs.ContainsKey(id))
                return false;
            buffs.Add(id, new Buff(value, type));
            switch (type)
            {
                default:
                case BuffType.Flat:
                    Set(Add(value, this.value));
                    break;
                case BuffType.Percent:
                    Set(ApplyThisBuff(this.value, value), false);
                    break;
            }
            return true;
        }

        public bool RemoveBuff(string id)
        {
            if (!buffs.ContainsKey(id))
                return false;

            Buff buff = buffs[id];
            switch (buff.type)
            {
                default:
                case BuffType.Flat:
                    Set(Sub(buff.value, this.value));
                    break;
                case BuffType.Percent:
                    Set(ApplyThisBuff(this.value, buff.value, true), false);
                    break;
            }
            buffs.Remove(id);
            return true;
        }

        private T ApplyAllBuffs(T val)
        {
            foreach (KeyValuePair<string, Buff> buff in buffs)
            {
                if(buff.Value.type == BuffType.Percent)         //Equivaux à: (value * (100+buff)) / 100
                {
                    val = ApplyThisBuff(val, buff.Value.value);
                }
            }
            return val;
        }

        private T ApplyThisBuff(T val, T percentBuff, bool invert = false)
        {
            T oneHundred = (T)(object)100;
            T b = Add(percentBuff, oneHundred);
            if (!invert)
            {
                val = Multiply(b, val);
                val = Divide(oneHundred, val);
            }
            else
            {
                val = Multiply(oneHundred, val);
                val = Divide(b, val);
            }

            return val;
        }

        #region Private + -
        private T Add(T value, T to)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)((int)(object)value + (int)(object)to);
            else if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)value + (float)(object)to);
            else if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)value + (double)(object)to);
            return this;
        }
        private T Sub(T value, T to)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)((int)(object)to - (int)(object)value);
            else if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)to - (float)(object)value);
            else if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)to - (double)(object)value);
            return this;
        }
        private T Multiply(T value, T to)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)(Mathf.RoundToInt((int)(object)value * (int)(object)to));
            else if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)value * (float)(object)to);
            else if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)value * (double)(object)to);
            return this;
        }
        private T Divide(T value, T to)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)(Mathf.RoundToInt((float)(int)(object)to / (float)(int)(object)value));
            else if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)to / (float)(object)value);
            else if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)to / (double)(object)value);
            return this;
        }
        #endregion

        #region operator overloading

        public static implicit operator T(Stat<T> stat)
        {
            return stat.value;
        }
        public static implicit operator string (Stat<T> stat)
        {
            return stat.ToString();
        }
        public override bool Equals(object obj)
        {
            return this == obj.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return value.ToString();
        }

        static public bool IsNull(Stat<T> a)
        {
            object c = a ?? null;
            return c == null;
        }

        public static bool operator ==(Stat<T> a, string b)
        {
            if (IsNull(a))
            {
                return b == null;
            }
            return a.ToString() == b;
        }
        public static bool operator !=(Stat<T> a, string b)
        {
            return !(a == b);
        }

        #endregion
    }
}
