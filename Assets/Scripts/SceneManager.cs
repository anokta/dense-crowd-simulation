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
            AgentManager.GetAgent(i).rigidbody.velocity = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        }
    }
}