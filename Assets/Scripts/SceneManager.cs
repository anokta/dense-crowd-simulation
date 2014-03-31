using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SceneManager : MonoBehaviour
{
    public Texture2D mapTexture;
    
    MapLoader mapLoader;
    AgentManager agentManager;

    Obstacle[] obstacles;

    Agent testAgent;

    void Start()
    {
        mapLoader = GetComponent<MapLoader>();
        mapLoader.LoadMapIntoScene(mapTexture);

        agentManager = GetComponent<AgentManager>();
        agentManager.LoadAgentsIntoScene();

        obstacles = FindObjectsOfType<Obstacle>();

        testAgent = agentManager.GetAgent(0);
        agentManager.GetAgent(0).controlled = true;
    }

    void Update()
    {
        //float x = Input.GetAxisRaw("Horizontal") * 5.0f;
        //float y = Input.GetAxisRaw("Vertical") * 5.0f;

        //testAgent.NetForce += new Vector3(x, 0.0f, y);

        //for (int i = 0; i < agentManager.GetAgentCount(); ++i)
        //{
        //    //if (Vector3.Distance(agentManager.GetAgent(i).Target, agentManager.GetAgent(i).Position) > 0.5f)
        //    //    agentManager.GetAgent(i).Velocity = agentManager.GetAgent(i).Target - agentManager.GetAgent(i).Position;
        //    //else
        //    //    agentManager.GetAgent(i).Velocity = Vector3.zero;
        //}
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(50, 50, 150, 75), "Set Random Targets"))
        {
            agentManager.SetRandomTargets();
        }
    }

    void LateUpdate()
    {
        agentManager.ResolveCollision(obstacles);
    }
}