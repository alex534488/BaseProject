using System;
using System.Collections;
using UnityEngine.Events;

namespace CCC.Utility
{
    [System.Serializable]
    public class Stat<T>
    {
        public class StatEvent : UnityEvent<T> { };
        public enum BoundMode
        {
            Cap, MaxLoop, MinLoop, BidirectionalLoop
        }

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
        public StatEvent onSet = new StatEvent();
        public StatEvent onMinReached = new StatEvent();
        public StatEvent onMaxReached = new StatEvent();
        public StatEvent onMinSet = new StatEvent();
        public StatEvent onMaxSet = new StatEvent();
        public BoundMode boundMode = BoundMode.Cap;

        public Stat(T value)
        {
            Set(value);
        }

        public Stat(T value, IComparable min, IComparable max, BoundMode boundMode)
        {
            this.boundMode = boundMode;
            this.min = min;
            this.min = max;
            Set(value);
        }

        public Stat<T> Set(T value)
        {
            if (value is IComparable)                           // Can be checked
            {
                if (min != null && min.CompareTo(value) > 0)            // Check min
                {
                    if (boundMode == BoundMode.MinLoop || boundMode == BoundMode.BidirectionalLoop)
                        QuickSet(Sub(Sub(value, MIN), MAX)); // équivaut à MAX - (MIN - value)
                    else
                        QuickSet(MIN);

                    onMinReached.Invoke(value);
                }
                else if (max != null && max.CompareTo(value) < 0)       // Check max
                {
                    if (boundMode == BoundMode.MaxLoop || boundMode == BoundMode.BidirectionalLoop)
                        QuickSet(Add(Sub(MAX, value), MIN)); // équivaut à MIN + (value - MAX)
                    else
                        QuickSet(MAX);
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
            //dynamic a = value;
            //dynamic b = to;
            //return a + b;
            return this;
        }
        private T Sub(T value, T to)
        {
            //dynamic a = value;
            //dynamic b = to;
            //return b - a;
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
