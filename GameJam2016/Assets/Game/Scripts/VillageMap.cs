using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageMap {

    private List<Village> allVillage;

    private Noeud[][] map;
    private int mapSize = 5;
    private int mid = 3;
    private int[] coordXInit = { 0, 1, 0, -1, 0, 1, 2, 1, 0, -1, -2, -1 };
    private int[] coordYInit = { 1, 0, -1, 0, 2, 1, 0, -1, -2, -1, 0, 1 };


    
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
    }

    //Prend une liste de village et les place sur la carte logique.
    private void placeVillage(Village[] listVillage)
    {
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
                //North
                if(y+1<mapSize)
                {
                    map[x][y].addConnection(map[x][y+1], direction.North);
                }
                //South
                if(y-1>=0)
                {
                    map[x][y].addConnection(map[x][y-1], direction.South);
                }
                //East
                if(x+1<mapSize)
                {
                    map[x][y].addConnection(map[x+1][y], direction.East);
                }
                if(x-1>=0)
                {
                    map[x][y].addConnection(map[x-1][y], direction.West);
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
                if(map[x][y].isFront())
                {
                    ret.Add(map[x][y].getVillage());
                }
            }
        }
        return ret;
    }
}


public class Noeud
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
        if(connection[(int)direction.North] == null || connection[(int)direction.East] == null 
            || connection[(int)direction.South] == null || connection[(int)direction.West] == null )
        {
            return true;
        }
        return false;
    }

    public void addConnection(Noeud voisin, direction dir)
    {
        if(connection[(int)dir] != null)
        {
            connection[(int)dir] = voisin;
        }
    }

    public void suppConnection(direction dir)
    {
        if(connection[(int)dir]!=null)
        {
            Noeud temp = connection[(int)dir];
            connection[(int)dir] = null;
            switch (dir)
            {
                case direction.East:
                    connection[(int)dir].suppConnection(direction.West);
                    break;
                case direction.West:
                    connection[(int)dir].suppConnection(direction.East);
                    break;
                case direction.North:
                    connection[(int)dir].suppConnection(direction.South);
                    break;
                case direction.South:
                    connection[(int)dir].suppConnection(direction.North);
                    break;
            }
        }
    }

    public Village getVoisin(direction dir)
    {
        return connection[(int)dir].village;
    }


}

public enum direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

struct coord
{
    public int x;
    public int y;
    
    public coord(int xIn,int yIn)
    {
        x = xIn;
        y = yIn;
    }
};