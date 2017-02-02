using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StorylineEvent: UnityEvent<Storyline> { }
/// <summary>
/// Optional for each node:
/// <para></para>
///     - Implementation of id_Arrive(out Village source, out Village destination, out int value, out Resource_Type type)
///         {
///             ...
///         }
/// <para></para>
///     - Implementation of id_Choose(int choice, string nextNodeId)
///         {
///             ...
///         }
/// <para></para>
///     - Implementation of id_Character(out Game.Characters.IKit kit)
///         {
///             ...
///         }
/// </summary>
public abstract class Storyline : MonoBehaviour, INewDay {
    public UnityAction<Storyline> onComplete = null;
    public StoryGraph storyGraph;
    bool isComplete = false;

    public bool IsComplete
    {
        get
        {
            return isComplete;
        }
    }

    public string Tag
    {
        get
        {
            return storyGraph != null ? storyGraph.tag : "";
        }
    }

    protected void Complete()
    {
        isComplete = true;
        if(onComplete != null)onComplete.Invoke(this);
    }

    /// <summary>
    /// Initialise et lance le 'storygraph'
    /// </summary>
    public virtual void Init(UnityAction<Storyline> onComplete, StoryGraph.SaveState graphSave, object savedData = null)
    {
        this.onComplete = onComplete;
        if(storyGraph != null) storyGraph.Init(this, Complete, graphSave);
    }

    /// <summary>
    /// Execute NewDay sur le 'storygraph'
    /// </summary>
    public virtual void NewDay()
    {
        if (storyGraph != null) storyGraph.NewDay();
    }

    /// <summary>
    /// Appelé lors de la destruction de la storyline. On néttoie le mess qu'on a fait s'il y a lieu.
    /// </summary>
    public virtual void Terminate()
    {

    }

    /// <summary>
    /// Doit retourner les donnée à sauvegarder
    /// </summary>
    public virtual object GetSavedData()
    {
        return null;
    }

    /// <summary>
    /// Fait progresser la storyline, selon un choix de 0 à 2
    /// </summary>
    public void Progress(int choice)
    {
        storyGraph.Progress(choice);
    }
}
