using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour
{

    public GameObject agentPrefab;

    public int agentCount;

    List<Agent> agents;

    static AgentManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadAgentsIntoScene();
    }

    void Update()
    {

    }

    void LoadAgentsIntoScene()
    {
        Transform entityContainer = GameObject.Find("20 Entities").transform;

        Transform agentContainer = new GameObject("Agents").transform;
        agentContainer.parent = entityContainer.transform;

        agents = new List<Agent>();
        for (int i = 0; i < agentCount; ++i)
        {
            GameObject agent = GameObject.Instantiate(agentPrefab, new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)), Quaternion.identity) as GameObject;
            agent.transform.parent = agentContainer;
            agents.Add(agent.GetComponent<Agent>());
        }
    }

    public static int GetAgentCount()
    {
        return instance.agentCount;
    }

    public static Agent GetAgent(int i)
    {
        return instance.agents[i];
    }
}