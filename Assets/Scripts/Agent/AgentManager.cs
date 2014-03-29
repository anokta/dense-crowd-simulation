using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour
{
    public int agentCount = 100;

    public float minDistance = 1.0f;
    public float maxVelocity = 2.0f;

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
            agents[i].MaxVelocity = 2.0f;
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
            Collider[] hits = Physics.OverlapSphere(agents[i].Position, 2.0f * minDistance);
            if (hits.Length > 0)
            {
                for (int j = 0; j < hits.Length; ++j)
                {
                    if (hits[j] != agents[i].collider)
                    {
                        switch (hits[j].transform.tag)
                        {
                            case "Agent":
                                resolveAgentCollision(agents[i], hits[j].transform.position);
                                break;
                            case "Obstacle":
                                resolveObstacleCollision(agents[i], hits[j].transform.position);
                                break;
                        }
                    }
                }
            }
        }
    }

    void resolveAgentCollision(Agent a1, Vector3 p)
    {
        float distance = Vector3.Distance(a1.Position, p);

        ///if (distance < minDistance)
        //{
            a1.Position += Vector3.Normalize(a1.Position - p) * Mathf.Max(0, 2.0f * minDistance - distance);
        //}
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

    //static int CompareAgentsByPosition(Agent a1, Agent a2)
    //{
    //    float diff = a1.Position.sqrMagnitude - a2.Position.sqrMagnitude;
    //    if (diff < 0)
    //        return -1;
    //    else if (diff > 0)
    //        return 1;
    //    else
    //        return 0;
    //}
}