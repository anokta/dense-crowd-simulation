using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{
    static float kThresh = 0.5f;
    static float degreeThresh = 30.0f;

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
        set { previousVelocity = velocity; velocity = value; } //value.normalized * maxVelocity; }
    }

    Vector3 previousVelocity;
    public Vector3 PreviousVelocity
    {
        get { return previousVelocity; }
        set { previousVelocity = value; }
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

    Vector3 vForce;
    public Vector3 VForce
    {
        get { return vForce; }
        set { vForce = value; }
    }

    void Awake()
    {
        mass = 1.0f;

        agentTransform = transform;

        target = new Vector3(Random.Range(0.0f, 100.0f), 0.0f, Random.Range(0.0f, 100.0f));
    }

    void Update()
    {
        Vector3 vPref = new Vector3(); // (target - Position).normalized * maxVelocity;

        vForce = velocity + netForce / mass * Time.deltaTime;

        if (netForce.sqrMagnitude > 0)
        {
            Vector3 fConstraint = netForce.normalized;
            float fEquals = Vector3.Dot(fConstraint, vForce);

            double[,] a = new double[,] { { 2, 0 }, { 0, 2 } };
            double[] b = new double[] { -2 * vPref.x, -2 * vPref.z };
            double[] v;
            double[,] c = { { fConstraint.x, fConstraint.z, fConstraint.x * vForce.x + fConstraint.z + vForce.z } };
            int[] ct = { 1 };

            alglib.minqpstate state;
            alglib.minqpreport rep;

            alglib.minqpcreate(2, out state);
            alglib.minqpsetquadraticterm(state, a);
            alglib.minqpsetlinearterm(state, b);
            alglib.minqpsetlc(state, c, ct);
            
            alglib.minqpoptimize(state);
            alglib.minqpresults(state, out v, out rep);

            if (rep.terminationtype == 4 || rep.terminationtype == 7)
            {
                velocity = new Vector3((float)v[0], 0.0f, (float)v[1]);
            }
        }

        if (velocity.sqrMagnitude > 0)
            agentTransform.rotation = Quaternion.LookRotation(velocity);

        agentTransform.position += Velocity.normalized * MaxVelocity * Time.deltaTime;

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

    public void CalculateDeceleration(List<Agent> agents)
    {
        if (Vector3.Dot((velocity - previousVelocity).normalized, velocity.normalized) < -Mathf.Cos(Mathf.Deg2Rad * degreeThresh))
        {
            Vector3 decelerationForce = kThresh * mass * (velocity - previousVelocity) / Time.deltaTime;

            netForce += decelerationForce;

            for (int i = 0; i < agents.Count; ++i)
            {
                if (Vector3.Dot(previousVelocity.normalized, (agents[i].Position - Position).normalized) < Mathf.Cos(2.0f * Mathf.Deg2Rad * degreeThresh))
                {
                    agents[i].NetForce -= 1.0f / agents.Count * decelerationForce;
                }
            }
        }
    }

    public void CalculateResistive(List<Agent> agents)
    {
        if (VForce.sqrMagnitude > 0.0f)
        {
            Vector3 resistiveForce = kThresh * mass * (velocity - vForce) / Time.deltaTime;

            netForce += resistiveForce;

            for (int i = 0; i < agents.Count; ++i)
            {
                if (Vector3.Dot(previousVelocity.normalized, (agents[i].Position - Position).normalized) < Mathf.Cos(2.0f * Mathf.Deg2Rad * degreeThresh))
                {
                    agents[i].NetForce -= 1.0f / agents.Count * resistiveForce;
                }
            }
        }
    }
}