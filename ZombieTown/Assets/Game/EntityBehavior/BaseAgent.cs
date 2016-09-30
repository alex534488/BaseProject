using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;

[RequireComponent(typeof(Seeker))]
public class BaseAgent : MonoBehaviour
{
    public float repathRate = 1;

    private float nextRepath = 0;

#if RVOImp
	private int agentID;
#endif

    private Vector3 target;
    private bool canSearchAgain = true;

    private bool canMove = false;

    private RVOController controller;

    Path path = null;

    List<Vector3> vectorPath;
    int wp;

#if RVOImp
	public bool astarRVO = true;
#endif

    public float moveNextDist = 1;
    Seeker seeker;

    MeshRenderer[] rends;

    //IAgent rvoAgent;
#if RVOImp
	public Vector3 position {
		get {
			if (astarRVO) return rvoAgent.InterpolatedPosition;
			else return RVO.Simulator.Instance.getAgentPosition3(agentID);
			return rvoAgent.InterpolatedPosition;
		}
	}
#endif

    void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    // Use this for initialization
    void Start()
    {
#if RVOImp
		if (!astarRVO) {
			agentID = RVO.Simulator.Instance.addAgent(new RVO.Vector2(transform.position.x, transform.position.z));
		} else {
			Pathfinding.RVO.Simulator sim = (FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator).GetSimulator();
			rvoAgent = sim.AddAgent(transform.position);
			rvoAgent.Radius = radius;
			rvoAgent.MaxSpeed = maxSpeed;
			rvoAgent.Height = height;
			rvoAgent.AgentTimeHorizon = agentTimeHorizon;
			rvoAgent.ObstacleTimeHorizon = obstacleTimeHorizon;
		}
#endif
        //SetTarget(-transform.position); // + transform.forward * 400);
        CanMove(false);
        controller = GetComponent<RVOController>();
    }

    public void SetTarget(Vector3 target)
    {
        CanMove(true);
        this.target = target;
        RecalculatePath();
    }

    public void CanMove(bool state)
    {
        canMove = state;
    }

    void RecalculatePath()
    {
        canSearchAgain = false;
        nextRepath = Time.time + repathRate * (Random.value + 0.5f);
        seeker.StartPath(transform.position, target, OnPathComplete);
    }

    void OnPathComplete(Path _p)
    {
        ABPath p = _p as ABPath;

        canSearchAgain = true;

        if (path != null) path.Release(this);
        path = p;
        p.Claim(this);

        if (p.error)
        {
            wp = 0;
            vectorPath = null;
            return;
        }


        Vector3 p1 = p.originalStartPoint;
        Vector3 p2 = transform.position;
        p1.y = p2.y;
        float d = (p2 - p1).magnitude;
        wp = 0;

        vectorPath = p.vectorPath;
        Vector3 waypoint;

        for (float t = 0; t <= d; t += moveNextDist * 0.6f)
        {
            wp--;
            Vector3 pos = p1 + (p2 - p1) * t;

            do
            {
                wp++;
                waypoint = vectorPath[wp];
                waypoint.y = pos.y;
            } while ((pos - waypoint).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1);
        }
    }

    void Update()
    {
        if (!canMove) return;

        if (Time.time >= nextRepath && canSearchAgain && target != null)
        {
            RecalculatePath();
        }

        Vector3 dir = Vector3.zero;

        Vector3 pos = transform.position;

        if (vectorPath != null && vectorPath.Count != 0)
        {
            Vector3 waypoint = vectorPath[wp];
            waypoint.y = pos.y;

            while ((pos - waypoint).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1)
            {
                wp++;
                waypoint = vectorPath[wp];
                waypoint.y = pos.y;
            }

            dir = waypoint - pos;
            float magn = dir.magnitude;
            if (magn > 0)
            {
                float newmagn = Mathf.Min(magn, controller.maxSpeed);
                dir *= newmagn / magn;
            }
        }

        controller.Move(dir);
    }
}
