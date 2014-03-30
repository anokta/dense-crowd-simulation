using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{
    static float elasticity = 1.0f;
    static float kThresh = 0.1f;

    static float degreeThreshD = 30.0f;
    static float degreeThreshR = 60.0f;

    public bool controlled; 

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

            //double[,] a = new double[,] { { 2, 0 }, { 0, 2 } };
            //double[] b = new double[] { -2 * vPref.x, -2 * vPref.z };
            //double[] v;
            //double[,] c = { { fConstraint.x, fConstraint.z, fConstraint.x * vForce.x + fConstraint.z + vForce.z } };
            //int[] ct = { 1 };

            //alglib.minqpstate state;
            //alglib.minqpreport rep;

            //alglib.minqpcreate(2, out state);
            //alglib.minqpsetquadraticterm(state, a);
            //alglib.minqpsetlinearterm(state, b);
            //alglib.minqpsetlc(state, c, ct);

            //alglib.minqpoptimize(state);
            //alglib.minqpresults(state, out v, out rep);

            //if (rep.terminationtype == 4 || rep.terminationtype == 7)
            //{
            //    velocity = new Vector3((float)v[0], 0.0f, (float)v[1]);
            //}
            //else
                velocity = vForce;
        }
        else
            

        if (controlled)
        {
            float x = Input.GetAxisRaw("Horizontal") * 5.0f;
            float y = Input.GetAxisRaw("Vertical") * 5.0f;

            velocity = new Vector3(x, 0.0f, y);
        }
        else
            velocity = vPref;

        if (velocity.sqrMagnitude > 0.0f)
            agentTransform.rotation = Quaternion.LookRotation(velocity);

        agentTransform.position += Velocity * Time.deltaTime;

        netForce = Vector3.zero;
    }

    public void PushAgents(List<Agent> agents)
    {
        for (int i = 0; i < agents.Count; ++i)
        {
            Vector3 pushForce = (1.0f / agents.Count) * Vector3.Scale(agents[i].Position, (agents[i].Position - (Position + Velocity * Time.deltaTime)).normalized);

            agents[i].NetForce += pushForce;
        }
    }

    public void ResolveAgentCollisions(List<Agent> agents)
    {
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

    public void ResolveObjectCollision(Vector3 position)
    {
        Vector3 normal = Position - position;

        if (Vector3.Dot(velocity, normal) < 0)
        {
            Vector3 collisionForce = normal * Vector3.Dot((-(1.0f + elasticity) * velocity) / (1.0f / mass), normal) / Time.deltaTime;

            netForce += collisionForce;
        }
    }

    public void CalculateDeceleration(List<Agent> agents)
    {
        Vector3 velocityDelta = velocity - previousVelocity;

        if (Vector3.Dot(velocityDelta.normalized, velocity.normalized) < -Mathf.Cos(Mathf.Deg2Rad * degreeThreshD))
        {
            Vector3 decelerationForce = kThresh * mass * velocityDelta / Time.deltaTime;

            netForce += decelerationForce;

            List<Agent> interactingAgents = new List<Agent>();

            for (int i = 0; i < agents.Count; ++i)
            {
                if (Mathf.Acos(Vector3.Dot(previousVelocity.normalized, (agents[i].Position - Position).normalized)) < 2.0f * Mathf.Deg2Rad * degreeThreshD)
                {
                    interactingAgents.Add(agents[i]);
                }
            }

            for (int i = 0; i < interactingAgents.Count; ++i)
            {
                interactingAgents[i].NetForce -= 1.0f / interactingAgents.Count * decelerationForce;
            }
        }
    }

    public void CalculateResistive(List<Agent> agents)
    {
        if (vForce.sqrMagnitude > 0.0f)
        {
            Vector3 resistiveForce = kThresh * mass * (velocity - vForce) / Time.deltaTime;

            netForce += resistiveForce;

            List<Agent> interactingAgents = new List<Agent>();

            for (int i = 0; i < agents.Count; ++i)
            {
                if (Mathf.Acos(Vector3.Dot(previousVelocity.normalized, (agents[i].Position - Position).normalized)) < 2.0f * Mathf.Deg2Rad * degreeThreshR)
                {
                    interactingAgents.Add(agents[i]);
                }
            }

            for (int i = 0; i < interactingAgents.Count; ++i)
            {
                agents[i].NetForce -= 1.0f / agents.Count * resistiveForce;
            }
        }
    }
}