using UnityEngine;
using System.Collections;

public class BattleLauncher {

    private int invaderPower;
    private int defenderPower;
    private float invaderBuff;
    private float defenderBuff;

    /// <summary>
    /// Lance et résoue une bataille où un enemie (ex: barbare) attaque un village
    /// </summary>
    static public BattleResult LaunchDefence(int barbarianPower, Village village)
    {
        //TODO, remplir le stuff

        // 1- Crée le battle setting en fonction du 'barbarianPower' et du 'village'
        Battle battle = new Battle(10, 10);

        // 2- Résoudre le combat
        BattleResult result = battle.Resolve();

        // 3- Réduire la quantité de soldat dans le village, dans les barbares, etc.

        //Renvoie le résultat de bataille à des fin informative
        return result;
    }

    /// <summary>
    /// Lance et résoue une bataille où le joueur attaque une région non-conquise (ex: barbare)
    /// </summary>
    static public BattleResult LaunchAttack(int barbarianPower, Village startVillage)
    {
        //TODO, remplir le stuff

        // 1- Crée le battle setting en fonction du 'barbarianPower' et du 'village'
        Battle battle = new Battle(10, 10);

        // 2- Résoudre le combat
        BattleResult result = battle.Resolve();

        // 3- Réduire la quantité de soldat dans le village, dans les barbares, etc.

        //Renvoie le résultat de bataille à des fin informative
        return result;
    }
}
