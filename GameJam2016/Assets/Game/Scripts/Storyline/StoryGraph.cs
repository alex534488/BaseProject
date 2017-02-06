using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(menuName = "Story/Graph")]
public class StoryGraph : ScriptableObject, INewDay
{
    [System.Serializable]
    public class SaveState
    {
        public string currentNodeId;
        public string lastExecutedNodeId;
        public int delay;
        public bool isComplete;

        public SaveState(string currentNodeId, string lastExecutedNodeId, int delay, bool isComplete)
        {
            this.currentNodeId = currentNodeId;
            this.lastExecutedNodeId = lastExecutedNodeId;
            this.delay = delay;
            this.isComplete = isComplete;
        }
    }
    [System.Serializable]
    public class Node
    {
        public Node() { }
        public Node(string id) { this.id = id; }
        public int arrivalDelay = 1;
        public string id = "";
        public RequestFrame request = null;
        public float x=0, y=0;
        public List<string> children = new List<string>();

        [System.NonSerialized]
        public StoryGraph graph = null;

        public void Execute()
        {
            //Parameters
            Village source = null;
            Village destination = Universe.Capitale;
            int value = 1;
            ResourceType type = ResourceType.custom;

            //Find method
            MethodInfo method = graph.controller.GetType().GetMethod(id + "_Arrive");

            //Invoke method
            if (method != null)
            {
                object[] parameters = { source, destination, value, type };
                method.Invoke(graph.controller, parameters);

                //Recapture parameters
                source = (Village)parameters[0];
                destination = (Village)parameters[1];
                value = (int)parameters[2];
                type = (ResourceType)parameters[3];
            }

            //Build request
            if (request != null)
            {
                string className = graph.controller.GetType().ToString();

                Command[] commands =
                {
                    Command.ProgressStoryline(className, "0"),
                    Command.ProgressStoryline(className, "1"),
                    Command.ProgressStoryline(className, "2")
                };
                Request rq = request.Build(source, destination, value, type, commands);
                MethodInfo characterGetter = graph.controller.GetType().GetMethod(id + "_Character");
                if (characterGetter != null)
                {
                    IKit kit = null;
                    object[] parameter = { kit };
                    characterGetter.Invoke(graph.controller, parameter);
                    kit = parameter[0] as IKit;
                    rq.SetCharacterKit(kit);
                }
                RequestManager.SendRequest(rq);
            }
        }
        public void OnChoose(int choice)
        {
            MethodInfo method = graph.controller.GetType().GetMethod(id + "_Choose");
            object[] parameters = { choice, id };
            if (method != null)
                method.Invoke(graph.controller, parameters);

            //Si nous n'avons pas 'branch' ailleur
            if (graph.currentNode == this)
            {
                // Branche au prochain noeud s'il existe
                if (choice < children.Count && choice >= 0)
                {
                    graph.BranchTo(children[choice]);
                }
                else
                {
                    graph.Complete();
                }
            }
        }
    }

    Node currentNode;
    public Node CurrentNode
    {
        get { return currentNode; }
    }
    Node lastExectuedNode;
    Object controller;
    int delay = 0;
    public string tag;
    [HideInInspector]
    public List<Node> nodes = new List<Node>();
    bool isComplete = false;

    UnityAction onComplete = null;

    public void Init(Object controller, UnityAction onComplete = null, SaveState saveState = null)
    {
        this.controller = controller;

        if (nodes == null || nodes.Count <= 0)
            throw new System.Exception("This story does not have any node.");

        foreach (Node node in nodes)
        {
            node.graph = this;
        }

        this.onComplete = onComplete;

        //Aucune sauvegarde
        if (saveState == null)
        {
            BranchTo(nodes[0]);
            isComplete = false;
        }
        //Avec sauvegarde
        else
        {
            currentNode = GetNode(saveState.currentNodeId);
            lastExectuedNode = GetNode(saveState.lastExecutedNodeId);
            isComplete = saveState.isComplete;
            delay = saveState.delay;
        }
    }

    public SaveState GetSaveState()
    {
        return new SaveState(
            currentNode != null ? currentNode.id : "", 
            lastExectuedNode != null ? lastExectuedNode.id : "",
            delay,
            isComplete
            );
    }

    void Complete()
    {
        if (isComplete)
            return;
        isComplete = true;
        if (onComplete != null) onComplete();
    }

    public void NewDay()
    {
        if (isComplete)
            return;
        
        delay--;

        //Execute le noeud apres le delai
        if (delay <= 0)
        {
            //Si on est sur la même node, même après l'avoir exécuté, c'est qu'on est arrivé à la fin
            if (lastExectuedNode != currentNode && currentNode != null)
            {
                Execute(currentNode);
            }
            else
            {
                Complete();
            }
        }
    }

    Node GetNode(string id, bool logErrors = true)
    {
        foreach (Node node in nodes)
        {
            if (node.id == id)
                return node;
        }
        if (logErrors)
            Debug.LogError("Failed to find node with id: " + id);
        return null;
    }

    public bool Has(string id)
    {
        return GetNode(id, false) == null ? false : true;
    }

    void Execute(Node node)
    {
        node.Execute();
        lastExectuedNode = node;
    }

    #region Branching
    public void BranchTo(string id, int delay)
    {
        Node temp = GetNode(id);
        BranchTo(temp, delay);
    }
    public void BranchTo(string id)
    {
        Node temp = GetNode(id);
        BranchTo(temp);
    }
    void BranchTo(Node node)
    {
        BranchTo(node, node.arrivalDelay);
    }
    void BranchTo(Node node, int delay)
    {
        if (node == null || controller == null)
            return;

        currentNode = node;
        this.delay = delay;

        if (delay == 0)
            Execute(node);
    }
    #endregion

    /// <summary>
    /// Retourne l'index de la relation parent-enfant. Si 'child' est le 3e enfant de 'parent' alors la fonction retourne 2.
    /// <para></para>
    /// Si 'child' n'est pas enfant de 'parent' alors la fonction retourne -1.
    /// </summary>
    public int FindParentship(Node child, string parentId)
    {
        Node parent = GetNode(parentId);

        int i = 0;
        foreach (string nextId in parent.children)
        {
            if (nextId == child.id)
                return i;
            i++;
        }
        return -1;
    }

    /// <summary>
    /// Trouve la relation de parenté de l'enfant/parent, si elle a lieux.
    /// </summary>
    /// <param name="child">Noeud enfant</param>
    /// <param name="parent">Le noeud parent. Sera null si le lien de parenté n'existe pas.</param>
    /// <param name="childrenIndex">L'index de l'enfant. Sera -1 si le lien de parenté n'existe pas.</param>
    public void FindParentship(Node child, out Node parent, out int childrenIndex)
    {
        foreach (Node node in nodes)
        {
            int i = 0;
            foreach (string nextId in node.children)
            {
                if (nextId == child.id)
                {
                    childrenIndex = i;
                    parent = node;
                    return;
                }
                i++;
            }
        }

        //Il ne sont pas parenté
        parent = null;
        childrenIndex = -1;
    }

    public void Progress(int choice)
    {
        if (currentNode == null)
        {
            Debug.Log("Cannot progress into storyline, current node is null");
            return;
        }
        currentNode.OnChoose(choice);
    }
}
