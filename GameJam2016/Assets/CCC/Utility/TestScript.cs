using UnityEngine;
using System.Collections;
using System;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    
    public void testVillageMap()
    {
        Empire emp = new Empire();
        Capitale cap = new Capitale(emp,12);
        Village[] tab = new Village[12];
        for(int i=0;i<12;i++)
        {
            tab[i] = new Village(emp,i,"Hey");
        }

        VillageMap temp  =new VillageMap(cap, tab);
        temp.testPrint();
        temp.removeVillage(tab[3]);
        temp.testPrint();

    }
    
}
