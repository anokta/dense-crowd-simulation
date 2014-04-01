using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SceneManager : MonoBehaviour
{
    public Texture2D[] mapTextures;
    
    MapLoader mapLoader;
    AgentManager agentManager;

    public static int sceneNumber = 0;

    string textDistance = "";
    float tempDistance = 0.0f;

    void Start()
    {
        sceneNumber = 0;

        mapLoader = GetComponent<MapLoader>();
        mapLoader.LoadMapIntoScene(mapTextures[sceneNumber]);

        agentManager = GetComponent<AgentManager>();
        agentManager.LoadAgentsIntoScene();

        agentManager.GetAgent(0).controlled = true;

        textDistance = AgentManager.initialDistance.ToString();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200, 50, 150, 75), "Initial Distance");

        textDistance = GUI.TextField(new Rect(200, 75, 50, 25), textDistance);
        bool success = float.TryParse(textDistance, out tempDistance);
        if (success)
        {
            AgentManager.initialDistance = Mathf.Max(0.5f, tempDistance);
        }
        
        if(GUI.Button(new Rect(25, 50, 150, 75), "Reset"))
        {
            agentManager.RestartScene();
            agentManager.GetAgent(0).controlled = true;
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
        else if (GUI.Button(new Rect(25, 500, 150, 50), "Exit"))
        {
            Application.Quit();
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
    }
}