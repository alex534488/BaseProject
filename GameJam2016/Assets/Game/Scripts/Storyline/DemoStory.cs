using UnityEngine;
using System.Collections;
using System;
using Game.Characters;

public class DemoStory : Storyline {

    /// <summary>
    /// Histoire:
    ///     Un villageois viens voir l'empereur parce que son cochon est malade.
    ///     Si le joueur choisi de donner des herbes médical au villageois, un soldat reviens 1 jours plus tard en disant qu'un villageois
    ///         est en rampage fou, SOUS L'INFLUENCE DE LA DROGUE !!!!  (perte de bouffe et de soldat) (fin)
    ///     Si le joueur ne donne pas les herbes, le villageois reviens le lendemain en redemandant de l'herbe. Il offre à l'emprereur tout
    ///         son argent en compensation.
    ///     Si le joueur ne donne pas les herbes, un soldat reviens 1 jours plus tard en disant qu'un villageois
    ///         s'est coupé les veine, par manque DE DROGUE !!!!  (perte de bonheur)
    /// 
    /// Ce que cette démo montre:
    ///     - Comment faire une mini-storyline à choix divergent
    ///     - Comment faire une request avec un type de personnage spécifique (soldat)
    ///     - Comment faire persister un même personnage à travers plusieurs request
    /// </summary>


    // Cette liste de requestFrame va etre bcp trop bordelique, il va falloir faire une plus belle structure (arbre decisionnel)

    [SerializeField]
    RequestFrame intro;
    [SerializeField]
    RequestFrame giveNoWeed;
    [SerializeField]
    RequestFrame giveWeed;
    [SerializeField]
    RequestFrame giveNoWeed2nd;

    Game.Characters.IKit mcKit;

    public override void Init()
    {
        mcKit = CharacterBank.GetKit(CharacterBank.StandardTags.Beggar);

        Request request = intro.Build(null, Empire.instance.capitale);
        request.SetCharacterKit(mcKit);

        RequestManager.SendRequest(request);
    }

    public override void NewDay()
    {

    }
}
