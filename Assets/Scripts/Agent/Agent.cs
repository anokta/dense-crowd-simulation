using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{
    
    Transform agentTransform;

    float mass;
    public float Mass
    {
        get { return mass; }
    }

    public Vector3 Position
    {
        get { return agentTransform.position; }
        set { agentTransform.position = value; }
    }

    Vector3 velocity;
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value.normalized * maxVelocity; }
    }

    float maxVelocity;
    public float MaxVelocity
    {
        get { return MaxVelocity; }
        set { maxVelocity = value; }
    }

    Vector3 target;
    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    Vector3 netForce;
    public Vector3 NetForce
    {
        get { return netForce; }
        set { netForce = value; }
    }

    void Awake()
    {
        mass = 1.0f;

        agentTransform = transform;

        target = new Vector3(Random.Range(0.0f, 100.0f), 0.0f, Random.Range(0.0f, 100.0f));
        velocity = (target - Position).normalized * 2.0f;
    }

    void Update()
    {
        Vector3 vPref = (target - Position).normalized * maxVelocity + Vector3.one * maxVelocity;

        Vector3 vForce = velocity + netForce / mass * Time.deltaTime + Vector3.one * maxVelocity;

        List<Vector3> A = new List<Vector3>();
        List<float> b = new List<float>();
        
        if (netForce.sqrMagnitude > 0)
        {
            Vector3 fConstraint = -netForce.normalized;
            A.Add(fConstraint);
            //A.Add(new Vector3(0, 1, 0));
            //A.Add(new Vector3(0, -1, 0));
            float fEquals = Vector3.Dot(fConstraint, vForce);
            b.Add(fEquals);
            //b.Add(0);
            //b.Add(0);
            //Debug.Log(A[0] + " " + b[0]);
           // Debug.Log(SimplexSolver.Solve(vPref, A, b));
            velocity = SimplexSolver.Solve(vPref + Vector3.one *maxVelocity, A, b) - new Vector3(maxVelocity, 0.0f, maxVelocity);
        }

        if(Velocity.sqrMagnitude > 0)
            agentTransform.rotation = Quaternion.LookRotation(Velocity);

        agentTransform.position += Velocity * Time.deltaTime;

        netForce = Vector3.zero;
    }

    public void PushAgents(List<Agent> agents)
    {
        for (int i = 0; i < agents.Count; ++i)
        {
            Vector3 normal = agents[i].Position - Position;

            Vector3 pushForce = normal * Vector3.Dot(agents[i].Position * (1.0f / agents.Count), (agents[i].Position - (Position + Velocity * Time.deltaTime)).normalized);

            agents[i].NetForce += pushForce;
        }
    }

    public void ResolveAgentCollisions(List<Agent> agents)
    {
        float elasticity = 1.0f;

        for (int i = 0; i < agents.Count; ++i)
        {
            Vector3 vRelative = velocity - agents[i].Velocity;
            Vector3 normal = Position - agents[i].Position;

            if (Vector3.Dot(vRelative, normal) < 0)
            {
                Vector3 collisionForce = normal * Vector3.Dot((-(1.0f + elasticity) * vRelative) / (1.0f / mass + 1.0f / agents[i].Mass), normal) / Time.deltaTime;

                netForce += collisionForce;
                agents[i].NetForce -= collisionForce;
            }
        }
    }

    public void CalculateDeceleration(Vector3 previousVelocity)
    {
        float kThresh = 0.25f;
        
        Vector3 decelerationForce = kThresh * mass * (velocity - previousVelocity) / Time.deltaTime;
    }
}