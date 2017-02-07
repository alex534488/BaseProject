using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DemoStory : Storyline
{

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

    //C'est très important de mettre les données en 'System.Serializable'
    [System.Serializable]
    public class Data
    {
        public IKit mcKit;
    }

    Data data;

    //Sera appelé au lancement de la storyline
    public override void Init(UnityAction<Storyline> onComplete, StoryGraph.SaveState graphSave, object savedData)
    {
        //Avec sauvegarde
        if (savedData != null)
        {
            data = savedData as Data;
        }
        //Sans sauvegarde
        else
        {
            data = new Data();
            data.mcKit = CharacterBank.GetRandomKit(); //CharacterBank.GetKit(CharacterBank.StandardTags.Beggar);
        }

        base.Init(onComplete, graphSave);
    }

    public override object GetSavedData()
    {
        return data;
    }

    //Sera appelé quand la storyline se termine (est détruite de la scène)
    //On enlève tous les effets, objets, etc. qui traine dans la scène en lien avec cette storyline
    public override void Terminate()
    {
        base.Terminate();
        //Rien à détruire dans ce cas
    }

    //Sera appelé 1 fois au début de chaque jour
    public override void NewDay()
    {
        base.NewDay();
    }

    public Request DemandeDeWeed_Arrive(Request requestToBeSent)
    {
        requestToBeSent.choix[0].condition = new Condition(ConditionType.AlwaysTrue);
        return requestToBeSent;
    }

    public IKit DemandeDeWeed_Character()
    {
        return data.mcKit;
    }

    public IKit Redemande_Character()
    {
        return data.mcKit;
    }
}
