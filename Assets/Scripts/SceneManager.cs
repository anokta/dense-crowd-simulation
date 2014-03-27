using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
        for (int i = 0; i < AgentManager.GetAgentCount(); ++i)
        {
            AgentManager.GetAgent(i).Velocity += new Vector3(Random.Range(-0.25f, 0.25f), 0.0f, Random.Range(-0.25f, 0.25f));
        }
    }
}