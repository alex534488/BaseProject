using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    public Text textUI;
    public Vector2 area;

    List<string> texts;
    int i = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) DayManager.main.Lose("Tu t'est simplement fait pété les chevilles.");

        //if (Input.GetKeyDown(KeyCode.T))
        //{

        //    string text = " Bonjour Empereur, votre récente nomination inquiète beaucoup de citoyens au sein de la capitale. En tant que conseiller laissez moi vous donnez quelques indications quant a la facon de gouverner un Empire."
        //        + " Chaque jour, des personnes provenant de tout votre empire vont venir vous voir pour faire des requêtes à l'État, libre à vous de les accepter ou non. Dépendemment de vos choix vous allez pouvoir constater les impacts."
        //        + " En effet, à votre gauche vous avez un indication de resources de votre Capitale. La quantité de soldat, le bonheur de la cité, la quantité de nourriture et d'or sont représenté.\n\n"
        //        + "À chaque tour vous avez une production de nourriture et d'or, mais les autres attributs peuvent diminuer aussi en fonction de votre gouvernance."
        //        + " Afin de bien gérer vos décissions et leur potentiel impacte sur l'Empire, vous avez a votre droite, vos trois conseillers. Ils servent tous à avoir de l'information sur les villages compris au sein de votre empire et leur situation. \n\n"
        //        + " Il vous est possible de demander ou de fournir des ressources à ces villages à l'aide de vos caravanes. À noter que les villes vont vous faire des demandes à l'occasion dépendemment de leur besoin."
        //        + " Également, il ne faut pas oublier les barbares peuvent attaquer à tout moment votre empire. Des villages pourraient se faire détruire s'il n'y a pas une armé assez suffisante.\n\n"
        //        + "Pour avoir une idée globale des chose vous pouvez envoyer un éclaireur qui récoltera de l'information sur les déplacements des barbares ce qui permettera au Ville de vous avertir s'ils sont en difficulté."
        //        + " Finalement, il ne faut pas oublier que lorsque vous avez terminer de gérer votre empire pour la journée en cours, il faut appuyer sur le bouton Prochain Jour afin de passer à la prochaine journée.\n\n"
        //        + "Un icone en haut au centre permet de signaler votre gouvernance a débuter depuis combien de temps."
        //        + "% Je crois que j'ai bien compris, je suis prêt à Gouverner!";
        //    texts = TextSplitter.Split(text, textUI, area, '%');
        //    i = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.N) && texts != null)
        //{
        //    textUI.text = texts[i];
        //    i++;
        //}
    }

}
