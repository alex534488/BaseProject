using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CCC.Utility
{
    public class Randomizer
    {
        public static void ShuffleList<T>(ref List<T> list)
        {
            int chosen = -1;
            T temp;
            for (int i = list.Count - 1; i >= 1; i--)
            {
                chosen = Random.Range(0, i);

                temp = list[chosen];
                list[chosen] = list[i];
                list[i] = temp;
            }
        }
    }
}
