using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Mode")]
public class Mode : ScriptableObject {

    // Description
    [Tooltip("Nom du mode de jeu, utile pour le retrouver rapidement")]
    public string nom = "default";

    // Mode de jeu
    [Tooltip("nombre entre 0 et plus qui definie la difficulté de la partie ")]
    public int Difficulty = 1; // 0 sera le tutoriel ? ou 0/1/2 facile/medium/hard
    [Tooltip("Es ce que la partie se termine avec la story line finale ou peut continuer par la suite sans fin")]
    public bool NoEnd = false;

    // Active ou desactive une resource
    public List<int> resources = new List<int>(); // liste bitmap des resources
                                                  // positionnement : gold|pop|etc. -> a determiner!

    // Ajout de d'autres options ici en les mettant public! (ne pas oublier les valeurs par default)

    // Constructeur d'un mode si on veut le faire en code
    /* A modifier quand les options de resources seront fixer
    Mode(// ressources/autres
         bool Tutorial, 
         int difficulty, 
         bool survival, 
         bool noend)
    {
        this.Difficulty = difficulty;
        this.NoEnd = noend;
    }
    */

    // Exemple de fonction pour savoir si une resource est active
    public bool IsGoldActive()
    {
        if (resources[0] == 1) return true;
        return false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Mode))]
public class ModeEditor : CCC.EditorUtil.AdvEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif

