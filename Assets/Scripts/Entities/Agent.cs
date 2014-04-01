using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour
{
    static float elasticity = 0.75f;
    static float kThresh = 0.1f;

    static float degreeThreshD = 30.0f;
    static float degreeThreshR = 60.0f;

    public static bool targetToggle = false;

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

    List<Vector3> uObstacle, nObstacle;
    List<Vector3> uAgent;

    void Awake()
    {
        mass = 1.0f;

        agentTransform = transform;

        target = new Vector3(Random.Range(0.0f, 100.0f), 0.0f, Random.Range(0.0f, 100.0f));

        uAgent = new List<Vector3>();
        uObstacle = new List<Vector3>();
        nObstacle = new List<Vector3>();
    }

    void Update()
    {
        Vector3 vPref = (target - Position).normalized * 0.5f * maxVelocity;
        if (!targetToggle || (target - Position).magnitude < 1.5f)
            vPref = Vector3.zero;

        vForce = velocity + netForce / mass * Time.deltaTime;
        if (netForce.sqrMagnitude > 0 || uObstacle.Count > 0)
        {
            Velocity = vForce;

            if (uObstacle.Count > 0)
            {
                for (int i = 0; i < uObstacle.Count; ++i)
                {
                    if (uObstacle[i].sqrMagnitude > 0)
                        Velocity = velocity + uObstacle[i];
                }
            }
            //if (uAgent.Count > 0)
            //{
            //    for (int i = 0; i < uAgent.Count; ++i)
            //    {
            //        if (uAgent[i].sqrMagnitude > 0)
            //            Velocity = velocity + 0.5f * uAgent[i];
            //    }
            //}
        }
        //if (netForce.sqrMagnitude > 0 || uAgent.Count > 0 || uObstacle.Count > 0)
        //{
        //    List<double[]> lcs = new List<double[]>();

        //    Vector3 fConstraint = netForce.normalized;
        //    float fEquals = Vector3.Dot(fConstraint, vForce);

        //    double[,] a = new double[,] { { 2, 0 }, { 0, 2 } };
        //    double[] b = new double[] { -2 * vPref.x, -2 * vPref.z };
        //    double[] v;
        //    double[] fc = { fConstraint.x, fConstraint.z, fConstraint.x * vForce.x + fConstraint.z + vForce.z };

        //    alglib.minqpstate state;
        //    alglib.minqpreport rep;

        //    alglib.minqpcreate(2, out state);
        //    alglib.minqpsetquadraticterm(state, a);
        //    alglib.minqpsetlinearterm(state, b);

        //    if (netForce.sqrMagnitude > 0)
        //        lcs.Add(fc);

        //    if (uAgent.Count > 0)
        //    {
        //        for (int i = 0; i < uAgent.Count; ++i)
        //        {
        //            Vector3 uNorm = uAgent[i].normalized;
        //            double[] orca1c = { uNorm.x, uNorm.z, (velocity.x + 0.5f * uAgent[i].x) * uNorm.x + (velocity.z + 0.5f * uAgent[i].z) * uNorm.z };
        //            lcs.Add(orca1c);
        //        }
        //    }
        //    if (uObstacle.Count > 0)
        //    {
        //        for (int i = 0; i < uObstacle.Count; ++i)
        //        {
        //            double[] orca2c = { nObstacle[i].x, nObstacle[i].z, (velocity.x + uObstacle[i].x) * nObstacle[i].x + (velocity.z + uObstacle[i].z) * nObstacle[i].z };
        //            lcs.Add(orca2c);
        //        }
        //    }

        //    double [,] lc = new double[lcs.Count, 3];
        //    int[] ct = new int[lcs.Count];
        //    for(int i=0; i<lcs.Count; ++i)
        //    {
        //        for(int j=0; j<3; ++j)
        //            lc[i, j] = lcs[i][j];

        //        ct[i] = 1;
        //    }

        //    alglib.minqpsetlc(state, lc, ct);

        //    alglib.minqpoptimize(state);
        //    alglib.minqpresults(state, out v, out rep);

        //    if (rep.terminationtype == 4 || rep.terminationtype == 7)
        //    {
        //        Velocity = new Vector3((float)v[0], 0.0f, (float)v[1]);
        //    }
        //    else
        //    {
        //        Velocity = Vector3.Lerp(velocity, vPref, 8.0f * Time.deltaTime);

        //    }
        //}
        else
        {
            Velocity = Vector3.Lerp(velocity, vPref, 8.0f * Time.deltaTime);
        }
        //else

        if (controlled)
        {
            float x = Input.GetAxisRaw("Horizontal") * 5.0f;
            float y = Input.GetAxisRaw("Vertical") * 5.0f;

            if(x != 0.0f || y != 0.0f)
                Velocity = new Vector3(x, 0.0f, y);
        }
            //else
            //    velocity = Vector3.Lerp(velocity, vPref, 8.0f * Time.deltaTime);

        if (velocity.sqrMagnitude > 0.1f)
            agentTransform.rotation = Quaternion.LookRotation(velocity);

        agentTransform.position += Vector3.Max(new Vector3(-maxVelocity, -maxVelocity, -maxVelocity), Vector3.Min(new Vector3(maxVelocity, maxVelocity, maxVelocity), velocity)) * Time.deltaTime;

        netForce = Vector3.zero;

        uAgent.Clear();
        uObstacle.Clear();
        nObstacle.Clear();
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
        Vector3 normal = (Position - position).normalized;

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

    public void AvoidAgent(Agent agent)
    {

        CalculateMinimumAvoidanceVector(agent.Position, agent.Velocity, false);
    }

    public void AvoidObstacle(Collider c)
    {

        CalculateMinimumAvoidanceVector(c.transform.position);
    }


    void CalculateMinimumAvoidanceVector(Vector3 p, Vector3 v = default(Vector3), bool obstacle = true)
    {
        Vector3 relativePosition = p - Position;
        Vector3 relativeVelocity = velocity - v;
        float distSq = relativePosition.sqrMagnitude;
        float combinedRadius = 1.0f;// obstacle ? 2.0f : 1.0f;
        float combinedRadiusSq = 1.0f;// obstacle ? 4.0f : 1.0f;
        float interpolateAmount = 0.2f;

        if (!Physics.Raycast(Position, velocity, relativePosition.magnitude + combinedRadius))
        {
            return;
        }

        Vector3 u;

        Vector3 w = relativeVelocity - interpolateAmount * relativePosition;
        float wLengthSq = w.sqrMagnitude;

        float dotProduct1 = Vector3.Dot(w, relativePosition);

        if (dotProduct1 < 0.0f && dotProduct1 * dotProduct1 > combinedRadiusSq * wLengthSq)
        {
            u = (combinedRadius * interpolateAmount - w.magnitude) * w.normalized;
        }
        else
        {
            float leg = Mathf.Sqrt(distSq - combinedRadiusSq);

            Vector3 direction;

            if ((relativePosition.x * w.z - relativePosition.z * w.x) > 0.0f)
            {
                direction = new Vector3(relativePosition.x * leg - relativePosition.z * combinedRadius, 0.0f, relativePosition.x * combinedRadius + relativePosition.z * leg) / distSq;
            }
            else
            {
                direction = -new Vector3(relativePosition.x * leg + relativePosition.z * combinedRadius, 0.0f, -relativePosition.x * combinedRadius + relativePosition.z * leg) / distSq;
            }

            float dotProduct2 = Vector3.Dot(relativeVelocity, direction);

            u = dotProduct2 * direction - relativeVelocity;
        }

        if (obstacle)
        {
            uObstacle.Add(u);
            nObstacle.Add(-relativePosition.normalized);
        }
        else
        {
            uAgent.Add(u);
        }
    }
}