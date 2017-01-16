using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace CCC.Utility
{
    public enum BoundMode
    {
        Cap, MaxLoop, MinLoop, BidirectionalLoop
    }
    [Serializable]
    public class Stat<T>
    {
        public class StatEvent : UnityEvent<T> { };
        
        T value;
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
        public BoundMode boundMode = BoundMode.Cap;

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

        public Stat<T> Set(T value)
        {
            if (value is IComparable)                           // Can be checked
            {
                if (min != null && (min.CompareTo(value) > 0 || min.Equals(value)))            // Check min
                {
                    if(min.CompareTo(value) > 0)
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
        private T Sub(object value, object to)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)((int)(object)to - (int)(object)value);
            else if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)to - (float)(object)value);
            else if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)to - (double)(object)value);
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

        public static bool operator ==(Stat<T> a, string b)
        {
            return a.ToString() == b;
        }
        public static bool operator !=(Stat<T> a, string b)
        {
            return !(a == b);
        }

        #endregion
    }
}
