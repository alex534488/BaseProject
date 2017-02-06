using UnityEngine;
using System.Collections;

public interface IKitMaker : CCC.Utility.ILottery
{
    IKit MakeKit();
    string Tag();
}
