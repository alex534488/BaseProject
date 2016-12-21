using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryGraphWindow : EditorWindow
{
    class GraphNode
    {
        public GraphNode(StoryGraph.Node node)
        {
            this.node = node;
        }
        public StoryGraph.Node node;
        public GraphNode parent;
        public List<GraphNode> children = new List<GraphNode>();
        public int totalChildren;
        public int totalChildLevel;

        public int Update_TotalChildren_Count()
        {
            totalChildren = 0;
            foreach (GraphNode child in children)
            {
                totalChildren += 1 + child.Update_TotalChildren_Count();
            }
            return totalChildren;
        }
        public int Update_TotalChildLevels_Count()
        {
            totalChildLevel = 0;
            foreach (GraphNode child in children)
            {
                int childTotal = child.Update_TotalChildLevels_Count();
                if (childTotal > totalChildLevel)
                    totalChildLevel = childTotal;
            }
            if (children.Count > 0)
                totalChildLevel += 1;
            return totalChildLevel;
        }
    }

    StoryGraph graph;
    Vector2 anchor = new Vector2(350, 100);
    bool save = false;
    GraphNode selectedNode;
    bool followGameProgress = false;
    ArrayList pathToSelectedNode = new ArrayList();
    List<GraphNode> graphNodes = null;
    double lastFocus = 0;
    Vector2 cellSize = new Vector2(100, 30);

    //Sprites
    Texture image_LongLeft;
    Texture image_ShortLeft;
    Texture image_Middle;
    Texture image_LongRight;
    Texture image_ShortRight;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/StoryGraph Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(StoryGraphWindow));
    }

    void Awake()
    {
        FetchImages();
    }

    int o = 0;
    bool wasPlaying = false;
    void Update()
    {
        if (Application.isPlaying)
        {
            //Si l'utilisateur viens tout juste de peser 'Play' -> suivre le progres de la partie = ON
            if (!wasPlaying)
                followGameProgress = true;

            //Si 'followGameProgress' -> met à jour la selection de la node à chaque ~32 frame
            if (followGameProgress && graph != null)
            {
                o++;
                if (o % 32 == 0)
                {
                    //Get où est rendu le joueur dans la storyline (s'il y en a une)
                    if (StorylineManager.Ongoing != null)
                        foreach (Storyline storyline in StorylineManager.Ongoing)
                        {
                            if (storyline.storyGraph == graph)
                                SelectNode(FindNode(graph.CurrentNode));
                        }
                }
            }
        }
        wasPlaying = Application.isPlaying;
    }
    void FetchImages()
    {
        image_LongLeft = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_LongLeft.png");
        image_LongRight = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_LongRight.png");
        image_Middle = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_Middle.png");
        image_ShortLeft = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_ShortLeft.png");
        image_ShortRight = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_ShortRight.png");
    }

    void OnNewGraph()
    {
        if (graph == null)
        {
            SelectNode(null);
            graphNodes = null;
            return;
        }

        ResetAnchor();


        graphNodes = new List<GraphNode>();

        if (graph.nodes == null)
        {
            graph.nodes = new List<StoryGraph.Node>();
        }
        //Ajoute tout les node
        foreach (StoryGraph.Node node in graph.nodes)
        {
            GraphNode graphNode = new GraphNode(node);
            graphNodes.Add(graphNode);
        }
        //Lie les nodes a leur enfant
        foreach (GraphNode graphNode in graphNodes)
        {
            graphNode.children = new List<GraphNode>();
            foreach (string nextId in graphNode.node.children)
            {
                GraphNode child = FindNode(nextId);
                if (child != null)
                {
                    graphNode.children.Add(child);
                    child.parent = graphNode;
                }
            }
        }
        UpdateGraphNodes();
    }

    void OnGUI()
    {
        save = false;

        Graph();
        Header();

        if (graph != null)
        {
            //Temporaire
            if (graph.nodes.Count <= 0)
            {
                graph.nodes.Add(new StoryGraph.Node());
            }

            if (selectedNode != null)
                Inspector(selectedNode);
        }

        GraphControl();

        if (save && graph != null)
        {
            EditorUtility.SetDirty(graph);
        }
    }

    void OnFocus()
    {
        lastFocus = EditorApplication.timeSinceStartup;
        if (graph != null && graph.nodes != null && graph.nodes.Count > 0 && graphNodes == null)
            OnNewGraph();
    }

    void GraphControl()
    {
        Event ev = Event.current;
        if (ev.type == EventType.MouseDrag && EditorApplication.timeSinceStartup - lastFocus > 0.05f)
        {
            anchor += ev.delta;
            Repaint();
        }
        else if (ev.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();
            if ((graphNodes == null || graphNodes.Count <= 0) && graph != null)
                menu.AddItem(new GUIContent("New/Node"), false, NewMotherNode);
            else
                menu.AddDisabledItem(new GUIContent("New/Node"));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Reset anchor"), false, ResetAnchor);
            if (selectedNode != null)
                menu.AddItem(new GUIContent("Deselect node"), false, DeselectNode);
            else
                menu.AddDisabledItem(new GUIContent("Deselect node"));
            menu.ShowAsContext();
            ev.Use();
        }
        else if (ev.type == EventType.KeyDown)
        {
            if (ev.keyCode == KeyCode.Escape)
            {
                DeselectNode();
                Repaint();
            }
        }
        else if (ev.type == EventType.ScrollWheel)
        {
            Vector2 deltaScale = cellSize;
            Vector2 scale = ev.delta.y > 0 ? new Vector2(-10, -3) : new Vector2(10, 3);
            cellSize += scale;
            if (cellSize.x < 40)
                cellSize = new Vector2(40, 12);

            deltaScale -= cellSize;

            Vector2 deltaMouse = new Vector2(ev.mousePosition.x - anchor.x, ev.mousePosition.y - anchor.y);
            float ratio = deltaMouse.magnitude / cellSize.magnitude;
            Vector2 move = ratio * deltaMouse.normalized * deltaScale.magnitude;

            anchor += ev.delta.y > 0 ? move : -move;

            Repaint();
        }

        Vector2 size = position.size;
        if (GUI.Button(new Rect(size.x - 90, 5, 85, 20), "ResetAnchor"))
        {
            ResetAnchor();
        }
        GUI.Label(new Rect(size.x - 110, 30, 105, 20), "'ESC' to deselect");
        if (Application.isPlaying)
            followGameProgress = GUI.Toggle(new Rect(5, size.y - 18, 150, 30), followGameProgress, "follow game progress");
    }

    void Header()
    {
        Vector2 pos = new Vector2(5, 5);
        Vector2 size = new Vector2(350, 50);

        EditorGUI.DrawRect(new Rect(pos.x, pos.y, size.x, size.y), Color.white);
        GUILayout.BeginArea(new Rect(pos.x + 5, pos.y + 5, size.x - 10, size.y - 10));

        GUILayout.Label("Graph", EditorStyles.boldLabel);
        StoryGraph temp = graph;
        graph = EditorGUILayout.ObjectField(graph, typeof(StoryGraph), false) as StoryGraph;

        if (temp != graph)
            OnNewGraph();

        GUILayout.EndArea();
    }

    void Inspector(GraphNode graphNode)
    {
        StoryGraph.Node node = graphNode.node;
        Vector2 pos = new Vector2(5, 60);
        Vector2 size = new Vector2(350, 84);

        EditorGUI.DrawRect(new Rect(pos.x, pos.y, size.x, size.y), new Color(0.9f, 0.9f, 0.9f));

        GUILayout.BeginArea(new Rect(pos.x + 5, pos.y + 5, size.x - 10, size.y - 10));

        GUILayout.Label("Node", EditorStyles.boldLabel);

        string newId = EditorGUILayout.TextField("Id", node.id);

        //S'il y a eu un changement d'Id, l'appliquer sur le parent aussi
        if (newId != node.id && !graph.Has(newId))
        {
            if (graphNode.parent != null)
            {
                int index = graphNode.parent.node.children.IndexOf(node.id);
                graphNode.parent.node.children[index] = newId;
            }
            node.id = newId;
        }

        node.request = EditorGUILayout.ObjectField("Request", node.request, typeof(RequestFrame), false) as RequestFrame;
        node.arrivalDelay = EditorGUILayout.IntField("Delay", node.arrivalDelay);

        save = true;
        GUILayout.EndArea();
    }

    void Graph()
    {
        if (graphNodes == null || graphNodes.Count <= 0)
            return;

        GraphNode start = graphNodes[0];
        DrawNode(start, anchor);
    }

    bool DrawNode(GraphNode graphNode, Vector2 position)
    {
        bool isPath = pathToSelectedNode != null && pathToSelectedNode.Contains(graphNode);

        float leftBorder = position.x - (cellSize.x / 2);
        float rightBorder = position.x + (cellSize.x / 2);
        float topBorder = position.y - (cellSize.y / 2);
        float botBorder = position.y + (cellSize.y / 2);

        GUI.color = Color.white;

        //Draw children
        if (isPath || selectedNode == graphNode)
        {
            float startX = 0;
            if (graphNode.children.Count > 0)
                startX = ((cellSize.x + 5) / 2) * (graphNode.children.Count - 1);
            Vector2 childPos = position + Vector2.up * (cellSize.y * 2) + Vector2.left * startX;

            int childIndex = 0;
            int totalChild = graphNode.children.Count;
            foreach (GraphNode child in graphNode.children)
            {
                if (child == selectedNode)
                    GUI.color = new Color(0.6f, 1, 0.6f);
                else if (pathToSelectedNode != null && pathToSelectedNode.Contains(child))
                    GUI.color = new Color(0.85f, 1, 0.85f);
                else
                    GUI.color = Color.white;

                //Determine l'image pour le lien
                Texture image = GetLinkImageFromIndex(childIndex, totalChild);

                //Draw children link
                GUI.DrawTexture(new Rect(childPos.x - cellSize.x, position.y, cellSize.x * 2, cellSize.y * 2), image); //childPos.x, position.y, Mathf.Abs(childPos.x - position.x), childPos.y - position.y

                GUI.color = Color.white;
                //Draw children button
                if (!DrawNode(child, childPos))
                    return false;

                childPos += Vector2.right * (cellSize.x + 5);
                childIndex++;
            }
        }

        //Draw self
        if (isPath)
            GUI.color = new Color(0.85f, 1, 0.85f);
        else if (selectedNode == graphNode)
            GUI.color = new Color(0.6f, 1, 0.6f);
        else if (graphNode.children.Count <= 0)
            GUI.color = new Color(1, 0.8f, 0.9f);

        string text = cellSize.x >= 60 ? graphNode.node.id : "";
        if (GUI.Button(new Rect(leftBorder, topBorder, cellSize.x, cellSize.y), text))
            SelectNode(graphNode);

        GUI.color = Color.white;

        //Draw + and x
        if (cellSize.x >= 95)
        {
            GUI.color = new Color(1, 1f, 0.5f);
            // + Button
            if (graphNode.children.Count < 3
            && GUI.Button(new Rect(position.x - 10, position.y + (cellSize.y / 2) + 2, 20, 20), "+"))
                NewNode(graphNode);

            GUI.color = new Color(1, 0.8f, 0.9f);
            // x Button
            if (graphNode.children.Count <= 0
            && GUI.Button(new Rect(rightBorder - 20, topBorder - 22, 20, 20), "x"))
            {
                RemoveNode(graphNode);
                return false;
            }

            GUI.color = Color.white;
        }

        //Draw childCount and levelCount
        if (cellSize.x >= 160)
        {
            GUI.Label(new Rect(leftBorder + 4, botBorder - 17, cellSize.x, cellSize.y),
                "Children: " + graphNode.totalChildren + "    Levels: " + graphNode.totalChildLevel);
        }
        return true;
    }

    void ResetAnchor()
    {
        Vector2 size = position.size;
        anchor = new Vector2(2 * size.x / 3, size.y / 3);
        cellSize = new Vector2(100, 30);
    }

    void NewMotherNode()
    {
        if (graphNodes == null)
            graphNodes = new List<GraphNode>();
        if (graphNodes.Count > 0 || graph == null)
            return;

        graph.nodes = new List<StoryGraph.Node>();
        graph.nodes.Add(new StoryGraph.Node("TBD_Top"));
        OnNewGraph();
        Repaint();
    }

    void NewNode(GraphNode parent)
    {
        //Crée la 'Node'
        StoryGraph.Node node = new StoryGraph.Node();

        //Choisie le nom
        string name = "TBD_";
        while (graph.Has(name))
            name += 'i';
        node.id = name;

        //Ajoute la node
        graph.nodes.Add(node);

        //Crée la 'GraphNode'
        GraphNode graphNode = new GraphNode(node);
        graphNodes.Add(graphNode);
        graphNode.parent = parent;

        //Ajoute la référence dans le parent
        parent.children.Add(graphNode);
        parent.node.children.Add(node.id);

        UpdateGraphNodes();

        SelectNode(graphNode);
    }

    void RemoveNode(GraphNode graphNode)
    {

        //Remove des parents
        StoryGraph.Node parent;
        int childIndex;
        graph.FindParentship(graphNode.node, out parent, out childIndex);
        if (parent != null)
        {
            parent.children.Remove(graphNode.node.id);
            GraphNode parentGraphNode = FindNode(parent);
            parentGraphNode.children.Remove(graphNode);

            //Selectionne le parent
            if (selectedNode == graphNode)
                SelectNode(parentGraphNode);
        }
        else if (selectedNode == graphNode)
            SelectNode(null);

        //Remove des listes
        graph.nodes.Remove(graphNode.node);
        graphNodes.Remove(graphNode);

        UpdateGraphNodes();
    }

    void DeselectNode()
    {
        SelectNode(null);
    }

    void SelectNode(GraphNode graphNode)
    {
        bool repaint = graphNode != selectedNode;

        selectedNode = graphNode;
        if (graphNode == null)
        {
            pathToSelectedNode = null;
        }
        else
        {
            BuildPathTo(graphNode);
        }
        if (repaint)
            Repaint();
    }

    void BuildPathTo(GraphNode node)
    {
        pathToSelectedNode = new ArrayList();

        if (!TreeRun(graphNodes[0], node))
            Debug.LogError("Error un tree run");
    }

    bool TreeRun(GraphNode node, GraphNode target)
    {
        if (node == target)
            return true;

        pathToSelectedNode.Add(node);

        foreach (GraphNode child in node.children)
        {
            if (TreeRun(child, target))
                return true;
        }
        pathToSelectedNode.Remove(node);
        return false;
    }

    GraphNode FindNode(StoryGraph.Node node)
    {
        foreach (GraphNode graphNode in graphNodes)
        {
            if (graphNode.node == node)
                return graphNode;
        }
        return null;
    }

    GraphNode FindNode(string id)
    {
        foreach (GraphNode graphNode in graphNodes)
        {
            if (graphNode.node.id == id)
                return graphNode;
        }
        return null;
    }

    Texture GetLinkImageFromIndex(int index, int childCount)
    {
        Texture image = image_Middle;
        switch (childCount)
        {
            default: break;
            case 2:
                image = index == 0 ? image_ShortLeft : image_ShortRight;
                break;
            case 3:
                if (index == 0)
                    image = image_LongLeft;
                else if (index == 1)
                    image = image_Middle;
                else
                    image = image_LongRight;
                break;
        }
        return image;
    }

    void UpdateGraphNodes()
    {
        foreach (GraphNode node in graphNodes)
        {
            node.Update_TotalChildLevels_Count();
            node.Update_TotalChildren_Count();
        }
    }
}