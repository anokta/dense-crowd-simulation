using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SceneManager : MonoBehaviour
{
    public Texture2D[] mapTextures;
    
    MapLoader mapLoader;
    AgentManager agentManager;

    public static int sceneNumber = 0;

    void Start()
    {
        sceneNumber = 0;

        mapLoader = GetComponent<MapLoader>();
        mapLoader.LoadMapIntoScene(mapTextures[sceneNumber]);

        agentManager = GetComponent<AgentManager>();
        agentManager.LoadAgentsIntoScene();
        agentManager.GetAgent(0).controlled = true;
        agentManager.GetAgent(0).renderer.material.color = Color.cyan;
        agentManager.GetAgent(0).transform.GetChild(0).renderer.material.color = agentManager.GetAgent(0).renderer.material.color;
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(25, 50, 150, 75), "Reset"))
        {
            agentManager.RestartScene();
            agentManager.GetAgent(0).controlled = true;
            agentManager.GetAgent(0).renderer.material.color = Color.cyan;
            agentManager.GetAgent(0).transform.GetChild(0).renderer.material.color = agentManager.GetAgent(0).renderer.material.color;
        }
        else if (GUI.Button(new Rect(25, 150, 150, 75), sceneNumber == 0 ? "Load Obstacles" : "Remove Obstacles"))
        {
            LoadTScene(1 - sceneNumber);
        }
        else if (GUI.Button(new Rect(25, 250, 150, 75), "Set Random Targets"))
        {
            agentManager.SetRandomTargets();
        }
        else if (GUI.Button(new Rect(50, 350, 125, 50), Agent.targetToggle ? "Follow Target ON" : "Follow Target OFF"))
        {
            Agent.targetToggle = !Agent.targetToggle;
        }
    }

    void LateUpdate()
    {
        agentManager.ResolveCollision();
    }

    void LoadTScene(int index)
    {
        sceneNumber = index;

        Destroy(GameObject.Find("Map"));
        mapLoader.LoadMapIntoScene(mapTextures[sceneNumber]);

        agentManager.RestartScene();

        agentManager.GetAgent(0).controlled = true;
        agentManager.GetAgent(0).renderer.material.color = Color.cyan;
        agentManager.GetAgent(0).transform.GetChild(0).renderer.material.color = agentManager.GetAgent(0).renderer.material.color;
    }
}