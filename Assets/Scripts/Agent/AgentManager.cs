using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour
{
    public int agentCount = 100;

    public float minDistance = 1.0f;
    public float maxVelocity = 2.0f;
    public float viewAngle = 60.0f;

    public GameObject agentPrefab;

    List<Agent> agents;

    public void LoadAgentsIntoScene()
    {
        Transform entityContainer = GameObject.Find("20 Entities").transform;

        Transform agentContainer = new GameObject("Agents").transform;
        agentContainer.parent = entityContainer.transform;

        agents = new List<Agent>();
        for (int i = 0; i < agentCount; ++i)
        {
            GameObject agent = GameObject.Instantiate(agentPrefab, new Vector3(Random.Range(0.0f, 10.0f), 0.0f, Random.Range(0.0f, 10.0f)), Quaternion.identity) as GameObject;
            agent.transform.parent = agentContainer;
            agents.Add(agent.GetComponent<Agent>());
            agents[i].MaxVelocity = maxVelocity;
        }
    }

    public int GetAgentCount()
    {
        return agentCount;
    }

    public Agent GetAgent(int i)
    {
        return agents[i];
    }

    public void ResolveCollision(Obstacle[] obstacles)
    {
        for (int i = 0; i < agentCount; ++i)
        {
            Collider[] hits = Physics.OverlapSphere(agents[i].Position, 4.0f * minDistance);
            if (hits.Length > 0)
            {
                List<Agent> neighbors = new List<Agent>();
                List<Agent> collidedNeighbors = new List<Agent>();

                for (int j = 0; j < hits.Length; ++j)
                {
                    if (hits[j].GetInstanceID() != agents[i].GetInstanceID())
                    {
                        switch (hits[j].transform.tag)
                        {
                            case "Agent":
                                Agent neighbor = hits[j].GetComponent<Agent>();

                                if (Vector3.Distance(agents[i].Position, neighbor.Position) < 2.0f * minDistance) // collision
                                {
                                    collidedNeighbors.Add(neighbor);
                                }
                                //else if (Vector3.Dot(agents[i].transform.forward, (neighbor.Position - agents[i].Position).normalized) < Mathf.Cos(Mathf.Deg2Rad * viewAngle)) // push
                                //{
                                //    Debug.Log("PUSH");
                                //    neighbors.Add(neighbor);
                                //}

                                break;
                            case "Obstacle":
                                resolveObstacleCollision(agents[i], hits[j].transform.position);
                                break;
                        }
                    }
                }

                agents[i].PushAgents(neighbors);
                agents[i].ResolveAgentCollisions(collidedNeighbors);
                agents[i].CalculateDeceleration(neighbors);
                agents[i].CalculateResistive(neighbors);
            }
        }
    }

    void resolveObstacleCollision(Agent a1, Vector3 p)
    {
        Vector3 agentPosition = a1.Position;

        float distanceX = Mathf.Abs(agentPosition.x - p.x);
        float distanceZ = Mathf.Abs(agentPosition.z - p.z);

        //if (distanceX < minDistance && distanceZ < minDistance)
        //{
        if (distanceX > distanceZ)
            agentPosition.x += Mathf.Sign(agentPosition.x - p.x) * Mathf.Max(0, 2.0f * minDistance - distanceX);
        else
            agentPosition.z += Mathf.Sign(agentPosition.z - p.z) * Mathf.Max(0, 2.0f * minDistance - distanceZ);
        a1.Position = agentPosition;
        //}
    }
}