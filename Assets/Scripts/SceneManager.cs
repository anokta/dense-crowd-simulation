using UnityEngine;
using System.Collections;

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
    }

    void Update()
    {
        //float x = Input.GetAxisRaw("Horizontal") * 5.0f;
        //float y = Input.GetAxisRaw("Vertical") * 5.0f;

        //testAgent.Velocity = new Vector3(x, 0.0f, y);

        for (int i = 0; i < agentManager.GetAgentCount(); ++i)
        {
            agentManager.GetAgent(i).Velocity = agentManager.GetAgent(i).Target - agentManager.GetAgent(i).Position;
        }
    }

    void LateUpdate()
    {
        agentManager.ResolveCollision(obstacles);
    }
}