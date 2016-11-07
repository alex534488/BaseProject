using UnityEngine;
using System.Collections;
using System;
//using System.Collections.Generic;
using UnityEngine.Events;

public class Stat<T>
{
    class StatEvent : UnityEvent<T> { };

    T value;
    public IComparable max = null;
    public IComparable min = null;
    StatEvent onSet = new StatEvent();
    StatEvent onMinReached = new StatEvent();
    StatEvent onMaxReached = new StatEvent();

    public Stat(T value)
    {
        Set(value);
    }

    public Stat(T value, IComparable min, IComparable max)
    {
        Set(value);
        this.min = min;
        this.max = max;
    }

    public Stat<T> Set(T value)
    {
        if(value is IComparable)
        {
            IComparable iValue = value as IComparable;
            if (min != null && min.CompareTo(value) > 0)
            {
                QuickSet((T)min);
                onMinReached.Invoke(value);
            }
            else if (max != null && max.CompareTo(value) < 0)
            {
                QuickSet((T)max);
                onMaxReached.Invoke(value);
            }
        }
        else
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
}
