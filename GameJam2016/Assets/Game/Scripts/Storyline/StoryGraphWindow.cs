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
        public List<GraphNode> parents = new List<GraphNode>();
        public List<GraphNode> children = new List<GraphNode>();
        public Vector2 Position(float scale = 1)
        {
            return new Vector2(node.x, node.y) * scale;
        }

        public void BreakParentLinks()
        {
            if (parents == null)
                return;
            foreach (GraphNode parent in parents)
            {
                parent.node.children.Remove(node.id);
                parent.children.Remove(this);
            }
            parents.Clear();
        }
        public void BreakParentLink(int index)
        {
            if (index < 0 || index >= parents.Count)
                return;

            parents[index].node.children.Remove(node.id);
            parents[index].children.Remove(this);
            parents.RemoveAt(index);
        }

        public void BreakChildLink(int index)
        {
            if (index < 0 || index >= children.Count)
                return;
            children[index].BreakParentLinks();
        }
        public void BreakChildLinks()
        {
            if (children == null)
                return;
            for (int i = 0; children.Count > 0;)
            {
                children[i].BreakParentLink(children[i].parents.IndexOf(this));
            }
        }

        public void AddChildLink(GraphNode child)
        {
            if (children.Contains(child) || child.parents.Contains(this))
                return;

            node.children.Add(child.node.id);
            children.Add(child);
            child.parents.Add(this);
        }
    }

    StoryGraph graph;
    Vector2 anchor = new Vector2(350, 100);
    bool save = false;
    GraphNode selectedNode;
    List<GraphNode> grabbedNodes = new List<GraphNode>();
    bool followGameProgress = false;
    ArrayList pathToSelectedNode = new ArrayList();
    List<GraphNode> graphNodes = null;
    List<GraphNode> drawnNodes = null;
    GUIStyle centered;
    GUIStyle righted;
    double lastFocus = 0;
    float scale;
    bool isFocused = false;
    double lastNodeClick = 0;
    GraphNode linkingFrom = null;
    Vector2 contextMenuPos = Vector2.zero;
    Vector2 cellSize
    {
        get
        { return new Vector2(80, 48) * scale; }
    }

    //Sprites
    Texture image_Cell;
    Texture image_GrabbedCell;
    Texture image_SelectedCell;
    Texture image_EndCell;
    Texture image_Anchor;
    Texture image_Add;
    Texture image_ArrowHead;

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
        centered = new GUIStyle();
        centered.alignment = TextAnchor.MiddleCenter;
        righted = new GUIStyle();
        righted.alignment = TextAnchor.MiddleRight;
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
        else
        {
            if (linkingFrom != null)
            {
                Repaint();
            }
        }
        wasPlaying = Application.isPlaying;
    }

    void FetchImages()
    {
        image_Cell = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_Cell.png");
        image_GrabbedCell = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_GrabbedCell.png");
        image_EndCell = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_EndCell.png");
        image_SelectedCell = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_SelectedCell.png");
        image_Anchor = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_Anchor.png");
        image_Add = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_Add.png");
        image_ArrowHead = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SG_ArrowHead.png");
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
        //Lie les nodes a leurs enfant/parents
        foreach (GraphNode graphNode in graphNodes)
        {
            graphNode.children = new List<GraphNode>();
            foreach (string nextId in graphNode.node.children)
            {
                GraphNode child = FindNode(nextId);
                if (child != null)
                {
                    graphNode.children.Add(child);
                    if (!child.parents.Contains(graphNode))
                        child.parents.Add(graphNode);
                }
            }
        }
    }

    void OnGUI()
    {
        save = false;

        DrawGraph();
        if (linkingFrom != null)
        {
            DrawLink();
        }
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
        isFocused = true;
        if (graph != null && graph.nodes != null && graph.nodes.Count > 0 && graphNodes == null)
            OnNewGraph();
    }

    void OnLostFocus()
    {
        isFocused = false;
        grabbedNodes.Clear();
    }

    void GraphControl()
    {
        Event ev = Event.current;
        if (ev.type == EventType.mouseDown)
        {
            GraphNode pointedNode = GetNodeAtPos(ev.mousePosition.x, ev.mousePosition.y);

            if (pointedNode == null)
            {
                grabbedNodes.Clear();
                if (ev.button == 0)
                    linkingFrom = null;
            }
            else
            {
                if(EditorApplication.timeSinceStartup - lastNodeClick < 0.25f)
                {
                    SelectNode(pointedNode);
                }

                //Link completion
                if (linkingFrom != null && linkingFrom != pointedNode)
                {
                    linkingFrom.AddChildLink(pointedNode);
                    linkingFrom = null;
                }
                
                //Multi-selection
                if (ev.control)
                {
                    if (!grabbedNodes.Contains(pointedNode))
                        grabbedNodes.Add(pointedNode);
                    else
                        grabbedNodes.Remove(pointedNode);
                }
                //Selection standard
                else
                {
                    if (!grabbedNodes.Contains(pointedNode))
                    {
                        grabbedNodes.Clear();
                        grabbedNodes.Add(pointedNode);
                    }
                }
                lastNodeClick = EditorApplication.timeSinceStartup;
            }
            Repaint();
        }
        else if (ev.type == EventType.MouseDrag && EditorApplication.timeSinceStartup - lastFocus > 0.05f && isFocused)
        {
            if (grabbedNodes == null || grabbedNodes.Count <= 0)
            {
                anchor += ev.delta;
            }
            else
            {
                foreach (GraphNode graphNode in grabbedNodes)
                {
                    graphNode.node.x += ev.delta.x / scale;
                    graphNode.node.y += ev.delta.y / scale;
                }
            }
            Repaint();
        }
        else if (ev.type == EventType.ContextClick)
        {
            contextMenuPos = ev.mousePosition;
            GenericMenu menu = new GenericMenu();
            if (graph != null)
                menu.AddItem(new GUIContent("New/Node"), false, NewNode);
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
            float modif = ev.delta.y > 0 ? 0.85f : 1.15f;
            float oldScale = scale;

            scale *= modif;
            if (scale > 5)
                scale = 5;
            if (scale < 0.25f)
                scale = 0.25f;

            modif = scale / oldScale;
            Vector2 deltaMouse = new Vector2(ev.mousePosition.x - anchor.x, ev.mousePosition.y - anchor.y);
            anchor += (1 - modif) * deltaMouse;

            Repaint();
        }

        Vector2 size = position.size;
        if (GUI.Button(new Rect(size.x - 90, 5, 85, 20), "ResetAnchor"))
        {
            ResetAnchor();
        }
        GUI.Label(new Rect(size.x - 136, 30, 130, 20), "Double-click to edit", righted);
        GUI.Label(new Rect(size.x - 111, 47, 105, 20), "'ESC' to deselect", righted);
        GUI.Label(new Rect(size.x - 156, 64, 150, 20), "Ctrl-click to grab many", righted);
        GUI.Label(new Rect(size.x - 156, 81, 150, 20), "Click on arrow head to break link", righted);
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
            foreach (GraphNode parent in graphNode.parents)
            {
                int index = parent.node.children.IndexOf(node.id);
                parent.node.children[index] = newId;
            }
            node.id = newId;
        }

        node.request = EditorGUILayout.ObjectField("Request", node.request, typeof(RequestFrame), false) as RequestFrame;
        node.arrivalDelay = EditorGUILayout.IntField("Delay", node.arrivalDelay);

        save = true;
        GUILayout.EndArea();
    }

    void DrawLink()
    {
        Handles.BeginGUI();

        Vector2 adjustedPosition = new Vector2(linkingFrom.node.x, linkingFrom.node.y) * scale + anchor;

        Vector2 adjustedExit = adjustedPosition + (Vector2.right * cellSize.x / 2) + (Vector2.up * (linkingFrom.children.Count - 1) * cellSize.y / 4);
        Vector2 childAjustedEntry = Event.current.mousePosition;

        Vector2 startBezier = adjustedExit + (Vector2.right * Mathf.Max(childAjustedEntry.x - adjustedExit.x, 35 * scale));
        Vector2 endBezier = childAjustedEntry + (Vector2.left * Mathf.Max(childAjustedEntry.x - adjustedExit.x, 35 * scale));

        Handles.DrawBezier(adjustedExit, childAjustedEntry, startBezier, endBezier, new Color(0, 0.7294f, 1), null, 2);

        Handles.EndGUI();
    }

    void DrawGraph()
    {
        if (graphNodes == null || graphNodes.Count <= 0)
            return;

        //Draw Anchor
        GUI.DrawTexture(new Rect(anchor.x - (scale * 50), anchor.y - (scale * 50), scale * 100, scale * 100), image_Anchor);

        drawnNodes = new List<GraphNode>(graphNodes.Count);

        for (int i = 0; i < graphNodes.Count; i++)
        {
            GraphNode graphNode = graphNodes[i];
            if (!drawnNodes.Contains(graphNode))
                if (!DrawNode(graphNode))
                    break;
        }
    }

    bool DrawNode(GraphNode graphNode)
    {
        //bool isPath = pathToSelectedNode != null && pathToSelectedNode.Contains(graphNode);
        drawnNodes.Add(graphNode);
        Vector2 adjustedPosition = new Vector2(graphNode.node.x, graphNode.node.y) * scale + anchor;

        float leftBorder = adjustedPosition.x - (cellSize.x / 2);
        float rightBorder = adjustedPosition.x + (cellSize.x / 2);
        float topBorder = adjustedPosition.y - (cellSize.y / 2);
        float botBorder = adjustedPosition.y + (cellSize.y / 2);

        GUI.color = Color.white;

        //Draw children
        int c = -1;
        for (int i = 0; i < graphNode.children.Count; i++)
        {
            GraphNode child = graphNode.children[i];
            Handles.BeginGUI();

            Vector2 adjustedExit = adjustedPosition + (Vector2.right * cellSize.x / 2) + (Vector2.up * c * cellSize.y / 4);
            Vector2 childAjustedEntry = new Vector2(child.Position().x, child.Position().y) * scale + anchor + (Vector2.left * cellSize.x / 2);

            Vector2 startBezier = adjustedExit + (Vector2.right * Mathf.Max(childAjustedEntry.x - adjustedExit.x, 35 * scale));
            Vector2 endBezier = childAjustedEntry + (Vector2.left * Mathf.Max(childAjustedEntry.x - adjustedExit.x, 35 * scale));

            Handles.DrawBezier(adjustedExit, childAjustedEntry, startBezier, endBezier, new Color(0, 0.7294f, 1), null, 2);

            Handles.EndGUI();

            if (!drawnNodes.Contains(child))
                if (!DrawNode(child))
                    break;
            c++;
        }

        string text = cellSize.x >= 60 ? graphNode.node.id : "";

        //Cell
        Texture img = null;
        if (selectedNode == graphNode)
            img = image_SelectedCell;
        else if (grabbedNodes != null && grabbedNodes.Contains(graphNode))
            img = image_GrabbedCell;
        else if(graphNode.children.Count <= 0)
            img = image_EndCell;
        else
            img = image_Cell;

        GUI.DrawTexture(new Rect(leftBorder, topBorder, cellSize.x, cellSize.y), img);

        //ArrowHead entry point
        if (graphNode.parents.Count > 0)
            if (GUI.Button(
                new Rect(leftBorder - 9.5f * scale, topBorder + (cellSize.y / 2) - (5 * scale), 10 * scale, 10 * scale),
                image_ArrowHead,
                GUIStyle.none))
            {
                grabbedNodes.Clear();
                graphNode.BreakParentLinks();
            }

        //Link starter
        if (graphNode.node.children.Count < 3)
            if (GUI.Button(
                new Rect(adjustedPosition.x + (cellSize.x * 0.5f) - (6 * scale), topBorder + ((c + 2) * cellSize.y / 4) - (6 * scale), 12 * scale, 12 * scale),
                image_Add,
                GUIStyle.none))
            {
                grabbedNodes.Clear();
                linkingFrom = graphNode;
            }

        //Text
        GUI.Label(new Rect(leftBorder, topBorder, cellSize.x, cellSize.y), text, centered);

        GUI.color = Color.white;

        //Draw x
        if (cellSize.x >= 95)
        {
            GUI.color = new Color(1, 0.8f, 0.9f);
            // x Button
            if (GUI.Button(new Rect(leftBorder + 6, topBorder + 3, 20, 20), "X", GUIStyle.none))
            {
                grabbedNodes.Clear();
                RemoveNode(graphNode);
                return false;
            }

            GUI.color = Color.white;
        }

        return true;
    }

    void ResetAnchor()
    {
        Vector2 size = position.size;
        anchor = new Vector2(size.x / 2, size.y / 2);
        scale = 1.35f;
    }

    void NewNode()
    {
        GraphNode newGraphNode = NewNode((contextMenuPos - anchor) / scale);
        if (linkingFrom != null && newGraphNode != null)
        {
            linkingFrom.AddChildLink(newGraphNode);
            linkingFrom = null;
        }
    }

    GraphNode NewNode(Vector2 pos)
    {
        //Crée la 'Node'
        StoryGraph.Node node = new StoryGraph.Node();

        //Choisie le nom
        string name = "TBD_";
        while (graph.Has(name))
            name += 'i';
        node.id = name;
        node.x = pos.x;
        node.y = pos.y;

        //Ajoute la node
        graph.nodes.Add(node);

        //Crée la 'GraphNode'
        GraphNode graphNode = new GraphNode(node);
        graphNodes.Add(graphNode);

        SelectNode(graphNode);
        return graphNode;
    }

    void RemoveNode(GraphNode graphNode)
    {
        //Remove des parents
        graphNode.BreakParentLinks();
        graphNode.BreakChildLinks();

        if (selectedNode == graphNode)
            SelectNode(null);

        //Remove des listes
        graph.nodes.Remove(graphNode.node);
        graphNodes.Remove(graphNode);
    }

    void DeselectNode()
    {
        grabbedNodes.Clear();
        SelectNode(null);
    }

    void SelectNode(GraphNode graphNode)
    {
        bool repaint = graphNode != selectedNode;

        selectedNode = graphNode;
        if (repaint)
            Repaint();
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

    GraphNode GetNodeAtPos(float x, float y)
    {
        if (graphNodes == null)
            return null;
        foreach (GraphNode graphNode in graphNodes)
        {
            Vector2 pos = graphNode.Position() * scale + anchor;
            if (pos.x + (cellSize.x / 2) > x && pos.x - (cellSize.x / 2) < x)
                if (pos.y + (cellSize.y / 2) > y && pos.y - (cellSize.y / 2) < y)
                    return graphNode;
        }
        return null;
    }
}