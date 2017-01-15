using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

    public int nbTerritory = 8; // nombre de territoire dans la map

    // Maps
    private bool[][] adjacencyMap;
    private int[] teamMap;

    // Initialisation
    public void Start() {
        InitialMapConfiguration();
        teamMap = new int[nbTerritory];
        adjacencyMap = new bool[nbTerritory][];
        for (int i = 0; i < nbTerritory; i++)
        {
            adjacencyMap[i] = new bool[nbTerritory];
        }
    }

    /// <summary>
    /// Retourne tous les territoires adjacents aux territoires appartenant a une equipe
    /// </summary>
    public List<int> GetAdjacentEnemyTerritory(int team)
    {
        List<int> zone = new List<int>();
        List<int> temp = new List<int>();

        // Pour chaque territoire 
        for (int i = 0; i < teamMap.Length; i++)
        {
            // appartenant a l'equipe
            if (teamMap[i] == team)
            {
                // On obtient une liste des territoires adjacents
                temp = GetAdjacentPositions(i);
                // Pour chacun de ceux-ci
                for (int j = 0; j < temp.Count; j++)
                {
                    // On ajoute a la liste tous ceux appartenant a l'enemie
                    if (IsEnemyTerritory(team,temp[j]))
                    {
                        zone.Add(temp[j]);
                    }
                }
            }
        }
        return zone;
    }

    /// <summary>
    /// Determine si le territoire appartient a l'equipe
    /// </summary>
    private bool IsEnemyTerritory(int team, int position)
    {
        return teamMap[position] != team;
    }

    /// <summary>
    /// Permet d'obtenir les territoires adjacents a une position
    /// </summary>
    public List<int> GetAdjacentPositions(int position)
    {
        List<int> zone = new List<int>();

        // Pour chaque territoire
        for(int i = 0; i < adjacencyMap[position].Length; i++)
        {
            // adjacent a la position
            if(adjacencyMap[position][i] == true)
            {
                // on ajoute a la zone d'adjacence
                zone.Add(i);
            }
        }
        return zone;
    }

    /// <summary>
    ///  Retourne la liste des villages adjacents a tous les territoires
    /// </summary>
    public List<Village> GetAllAdjacentVillages(int position)
    {
        List<int> zone = new List<int>();
        List<Village> villages = new List<Village>();

        // Prenons tous les territoires adjacents aux territoires des barbares
        zone = GetAdjacentEnemyTerritory(2);

        // et pour chacun
        for (int i = 0; i < zone.Count; i++)
        {
            // allons chercher le village correspondant et ajoutons le a la liste
            villages.Add(World.main.empire.GetVillage(zone[i]));
        }
        return villages;
    }

    /// <summary>
    ///  Retourne la liste des villages adjacents a un territoire
    /// </summary>
    public List<Village> GetAdjacentVillage(int position)
    {
        List<int> zone = new List<int>();
        List<Village> villages = new List<Village>();

        // Prenons tous les territoires adjacents a la position
        zone = GetAdjacentPositions(position);

        // et pour chacun
        for(int i = 0; i < zone.Count; i++)
        {
            // s'ils ont un village, ajoute le a la liste
            if (IsEnemyTerritory(2, zone[i])) villages.Add(World.main.empire.GetVillage(zone[i]));
        }
        return villages;
    }

    /// <summary>
    /// Pour obtenir le nom des territoires
    /// </summary>
    public string PositionToRegionName(int position)
    {
        switch (position)
        {
            default:
                return "NaT"; // Not a Territory
            case 0:
                return "Mediolanum";
            case 1:
                return "Cremona";
            case 2:
                return "Aquileia";
            case 3:
                return "Neopolis";
            case 4:
                return "Tarentum";
            case 5:
                return "Hispalis";
            case 6:
                return "Christinea";
            case 7:
                return "Lutetia";
            // Autres suggestions de nom PARTISCUM, MONAECUM, AMSTELODAMUM, EBURACUM
        }
    }

    /// <summary>
    /// Change l'appartenance d'un territoire
    /// </summary>
    public void ChangeTerritoryOwner(int position, int team)
    {
        teamMap[position] = team;
    }
	
    // Appartenance initiale des territoires
    private void InitialTeamConfiguration()
    {
        // Team 1 Joueur, Team 2 Barbares
        teamMap[0] = 2;
        teamMap[1] = 2;
        teamMap[2] = 1;
        teamMap[3] = 2;
        teamMap[4] = 1;
        teamMap[5] = 1;
        teamMap[6] = 2;
        teamMap[7] = 2;
    }

    // Configuration de la map
    private void InitialMapConfiguration()
    {
        // on debute en haut jusqu'en bas et on prend en consideration que
        // les autres sont a false

        adjacencyMap[0][1] = true; // le territoire 0 a le territoire 1 d'adjacent

        adjacencyMap[1][0] = true;
        adjacencyMap[1][2] = true;
        adjacencyMap[1][3] = true;

        adjacencyMap[2][1] = true;
        adjacencyMap[2][3] = true;
        adjacencyMap[2][4] = true;  // 4 est la capitale
        adjacencyMap[2][5] = true;

        adjacencyMap[3][1] = true;
        adjacencyMap[3][2] = true;
        adjacencyMap[3][5] = true;
        adjacencyMap[3][6] = true;

        adjacencyMap[4][2] = true;
        adjacencyMap[4][5] = true;

        adjacencyMap[5][2] = true;
        adjacencyMap[5][4] = true;
        adjacencyMap[5][3] = true;
        adjacencyMap[5][6] = true;

        adjacencyMap[6][7] = true;
        adjacencyMap[6][5] = true;
        adjacencyMap[6][3] = true;

        adjacencyMap[7][6] = true;
    }
}
