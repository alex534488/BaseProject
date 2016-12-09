using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility
{
    public interface ILottery
    {
        int Weight();
    }
    public class Lottery
    {
        class LotteryItem
        {
            public LotteryItem(object obj, int weight)
            {
                this.obj = obj;
                this.weight = weight;
            }
            public object obj = null;
            public int weight = 1;
        }

        ArrayList list = new ArrayList();
        public void Add(ILottery item)
        {
            Add(item, item.Weight());
        }
        public void Add(object item, int weight)
        {
            list.Add(new LotteryItem(item, weight));
        }

        public object Pick()
        {
            if (list.Count <= 0)
            {
                Debug.LogError("No lottery item to pick from. Add some before picking.");
                return null;
            }

            int totalWeight = 0;
            foreach (LotteryItem item in list)
            {
                totalWeight += item.weight;
            }

            int ticket = Random.Range(0, totalWeight);
            int currentWeight = 0;
            foreach (LotteryItem item in list)
            {
                currentWeight += item.weight;
                if (ticket < currentWeight)
                    return item.obj;          //Devrais toujours return ici
            }

            Debug.LogError("Error in lotery.");
            return null;
        }
    }
}
