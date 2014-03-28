using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public Texture2D mapTexture;

    MapLoader mapLoader;
    AgentManager agentManager;

    void Start()
    {
        mapLoader = GetComponent<MapLoader>();
        mapLoader.LoadMapIntoScene(mapTexture);

        agentManager = GetComponent<AgentManager>();
        agentManager.LoadAgentsIntoScene();
    }

    void Update()
    {
        //float x = Input.GetAxisRaw("Horizontal") * 5.0f;
        //float y = Input.GetAxisRaw("Vertical") * 5.0f;

        //agentManager.GetAgent(0).Velocity = new Vector3(x, 0.0f, y);

        for (int i = 0; i < agentManager.GetAgentCount(); ++i)
        {
            agentManager.GetAgent(i).Velocity += new Vector3(Random.Range(-0.25f, 0.25f), 0.0f, Random.Range(-0.25f, 0.25f));
        }
    }
}