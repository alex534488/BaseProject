using System;
using System.Collections;
using UnityEngine.Events;

namespace CCC.Utility
{
    [System.Serializable]
    public class Stat<T>
    {
        public class StatEvent : UnityEvent<T> { };

        T value;
        IComparable max = null;
        IComparable min = null;
        public IComparable MAX
        {
            get { return max; }
            set { max = value; onMaxSet.Invoke((T)value); }
        }
        public IComparable MIN
        {
            get { return min; }
            set { min = value; onMinSet.Invoke((T)value); }
        }
        public StatEvent onSet = new StatEvent();
        public StatEvent onMinReached = new StatEvent();
        public StatEvent onMaxReached = new StatEvent();
        public StatEvent onMinSet = new StatEvent();
        public StatEvent onMaxSet = new StatEvent();

        public Stat(T value)
        {
            Set(value);
        }

        public Stat(T value, IComparable MIN, IComparable MAX)
        {
            this.MIN = MIN;
            this.MAX = MAX;
            Set(value);
        }

        public Stat<T> Set(T value)
        {
            if (value is IComparable)                           // Can be checked
            {
                IComparable iValue = value as IComparable;
                if (min != null && min.CompareTo(value) > 0)            // Check min
                {
                    QuickSet((T)min);
                    onMinReached.Invoke(value);
                }
                else if (max != null && max.CompareTo(value) < 0)       // Check max
                {
                    QuickSet((T)max);
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

        public bool TestSet(IComparable value)
        {
            if (min != null && min.CompareTo(value) > 0) return false;
            else if (max != null && max.CompareTo(value) < 0) return false;
            return true;
        }

        public System.Type Type() { return typeof(T); }

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
