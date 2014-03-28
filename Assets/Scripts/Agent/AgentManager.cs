using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour
{
    public int agentCount = 100;

    public float minDistance = 1.0f;

    public GameObject agentPrefab;

    List<Agent> agents;

    void Awake()
    {
        agents = new List<Agent>();
    }

    public void LoadAgentsIntoScene()
    {
        Transform entityContainer = GameObject.Find("20 Entities").transform;

        Transform agentContainer = new GameObject("Agents").transform;
        agentContainer.parent = entityContainer.transform;

        for (int i = 0; i < agentCount; ++i)
        {
            GameObject agent = GameObject.Instantiate(agentPrefab, new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)), Quaternion.identity) as GameObject;
            agent.transform.parent = agentContainer;
            agents.Add(agent.GetComponent<Agent>());
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
}