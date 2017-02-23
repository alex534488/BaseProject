using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility
{
    [System.Serializable]
    public class RandomList<T>
    {
        [SerializeField]
        List<T> list;

        public RandomList()
        {
            list = new List<T>();
        }

        //Returns a random element (from 0 to count -1). Then put the element at the end of the list, preventing it from being selected twice in a row.
        public T Pick()
        {
            if (list.Count > 1)
            {
                int topIndex = list.Count - 1;

                PlaceInLast(UnityEngine.Random.Range(0, topIndex));

                return list[topIndex];
            }
            else
            {
                return list[0];
            }
        }

        public void PlaceInLast(T item)
        {
            if (list.Contains(item))
                PlaceInLast(list.IndexOf(item));
        }

        private void PlaceInLast(int index)
        {
            if (list.Count <= 1)
                return;

            int topIndex = list.Count - 1;
            T temp = list[topIndex];

            //Swap chosen element with element at end of list
            list[topIndex] = list[index];
            list[index] = temp;
        }
    }
    [System.Serializable]
    public class RandomIntList : RandomList<int> { }
    [System.Serializable]
    public class RandomStringList : RandomList<string> { }
    [System.Serializable]
    public class RandomFloatList : RandomList<float> { }
    [System.Serializable]
    public class RandomAudioCliptList : RandomList<AudioClip> { }
    [System.Serializable]
    public class RandomBooltList : RandomList<bool> { }
}
