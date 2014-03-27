using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour
{

    public GameObject agentPrefab;

    public int agentCount;

    List<Agent> agents;


    void Start()
    {
        GameObject agentContainer = new GameObject("Agents");
        agentContainer.transform.parent = GameObject.Find("20 Entities").transform;

        agents = new List<Agent>();
        for (int i = 0; i < agentCount; ++i)
        {
            GameObject agent = GameObject.Instantiate(agentPrefab, new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f)), Quaternion.identity) as GameObject;
            agent.transform.parent = agentContainer.transform;
            agents.Add(agent.GetComponent<Agent>());
        }
    }

    void Update()
    {

    }
}