using UnityEngine;
using System.Collections;

namespace Game.Characters
{
    public interface IKitMaker : CCC.Utility.ILottery
    {
        IKit MakeKit();
        string Tag();
    }
}
