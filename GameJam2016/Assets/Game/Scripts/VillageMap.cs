using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageMap {

    private class Noeud
    {
        Village village;
        Noeud[] connection;

        public Noeud(Village centre)
        {
            connection = new Noeud[4];
            village = centre;
        }

        public Village getVillage()
        {
            return village;
        }

        public bool isFront()
        {
            if (connection[(int)direction.North] == null || connection[(int)direction.East] == null
                || connection[(int)direction.South] == null || connection[(int)direction.West] == null)
            {
                return true;
            }
            return false;
        }

        public int nbConnection()
        {
            int i = 0;
            if(connection[(int)direction.East]!=null)
            {
                i++;
            }
            if (connection[(int)direction.West] != null)
            {
                i++;
            }
            if (connection[(int)direction.South] != null)
            {
                i++;
            }
            if (connection[(int)direction.North] != null)
            {
                i++;
            }
            return i;
        }

        public void addConnection(Noeud voisin, direction dir)
        {
            if (connection[(int)dir] == null)
            {
                connection[(int)dir] = voisin;
            }
        }

        public void suppConnection(direction dir)
        {
            if (connection[(int)dir] != null)
            {
                Noeud temp = connection[(int)dir];
                connection[(int)dir] = null;
                switch (dir)
                {
                    case direction.East:
                        temp.suppConnection(direction.West);
                        break;
                    case direction.West:
                        temp.suppConnection(direction.East);
                        break;
                    case direction.North:
                        temp.suppConnection(direction.South);
                        break;
                    case direction.South:
                        temp.suppConnection(direction.North);
                        break;
                }
            }
        }

        public Village getVoisin(direction dir)
        {
            return connection[(int)dir].village;
        }


    }

    private List<Village> allVillage;
    private Noeud[][] map;
    private int mapSize = 5;
    private int mid = 2;

    public VillageMap(Capitale cap,Village[] tabVillage)
    {
        allVillage = new List<Village>();

        map = new Noeud[mapSize][];
        for(int i = 0;i< mapSize; i++)
        {
            map[i] = new Noeud[mapSize];
        }

        allVillage.Add(cap);
        Noeud tempNoeud = new Noeud(cap);
        map[mid][mid] = tempNoeud;

        placeVillage(tabVillage);
        makeConnection();
        updateFrontVillage();
    }

    //Prend une liste de village et les place sur la carte logique.
    private void placeVillage(Village[] listVillage)
    {
        int[] coordXInit = { 0, 1, 0, -1, 0, 1, 2, 1, 0, -1, -2, -1 };
        int[] coordYInit = { 1, 0, -1, 0, 2, 1, 0, -1, -2, -1, 0, 1 };
        for(int i=0;i<12&&i<listVillage.Length;i++)
        {
            allVillage.Add(listVillage[i]);
            Noeud temp = new Noeud(listVillage[i]);
            map[coordXInit[i]+mid][coordYInit[i]+mid] = temp;
        }
    }

    //Fait la connection entre les village en fonction de leur voisins.
    private void makeConnection()
    {
        for(int x = 0;x<mapSize;x++)
        {
            for(int y = 0;y<mapSize;y++)
            {
                if(map[x][y] != null)
                {
                    //North
                    if (y + 1 < mapSize)
                    {
                        map[x][y].addConnection(map[x][y + 1], direction.North);
                    }
                    //South
                    if (y - 1 >= 0)
                    {
                        map[x][y].addConnection(map[x][y - 1], direction.South);
                    }
                    //East
                    if (x + 1 < mapSize)
                    {
                        map[x][y].addConnection(map[x + 1][y], direction.East);
                    }
                    //West
                    if (x - 1 >= 0)
                    {
                        map[x][y].addConnection(map[x - 1][y], direction.West);
                    }
                }
            }
        }
    }

    private List<Village> getFrontVillage()
    {
        List<Village> ret = new List<Village>();

        for(int x=0;x<mapSize;x++)
        {
            for(int y=0;y<mapSize;y++)
            {
                if(map[x][y] != null && map[x][y].isFront())
                {
                    ret.Add(map[x][y].getVillage());
                }
            }
        }
        return ret;
    }

    public void removeVillage(Village villageIn)
    {
        for(int x=0;x<mapSize;x++)
        {
            for(int y = 0;y<mapSize;y++)
            {
                if(map[x][y]!=null && map[x][y].getVillage()==villageIn)
                {
                    map[x][y].suppConnection(direction.North);
                    map[x][y].suppConnection(direction.South);
                    map[x][y].suppConnection(direction.East);
                    map[x][y].suppConnection(direction.West);
                    map[x][y] = null;
                    allVillage.Remove(villageIn);
                }
            }
        }
        updateFrontVillage();
    }

    private void updateFrontVillage()
    {
        List<Village> list = getFrontVillage();
        foreach(Village vil in list)
        {
            if(vil.isFrontier == false)
            {
                vil.OnBecomesFrontier();
            }  
        }
    }

    public void testPrint()
    {
        for(int x=0;x<mapSize;x++)
        {
            string tempStr = "";
            for(int y=0; y< mapSize; y++)
            {
                if(map[x][y]!=null)
                {
                    tempStr += map[x][y].getVillage().isFrontier;
                }
                else
                {
                    tempStr += "0";
                }
                tempStr += " , "; 
            }
            Debug.Log(tempStr);
        }

    }
}


public enum direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
