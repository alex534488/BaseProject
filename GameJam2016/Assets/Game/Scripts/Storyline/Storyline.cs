using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StorylineEvent: UnityEvent<Storyline> { }
public abstract class Storyline : MonoBehaviour, IInit, INewDay {
    public StorylineEvent onComplete = new StorylineEvent();

    public void Complete()
    {
        onComplete.Invoke(this);
    }

    abstract public void Init();
    abstract public void NewDay();
}
