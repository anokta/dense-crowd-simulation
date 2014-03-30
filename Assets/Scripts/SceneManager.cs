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


        //Vector3 max = new Vector3(8, 10, 7);
        //List<Vector3> A = new List<Vector3>();
        //A.Add(new Vector3(0, -2, 2));
        //A.Add(new Vector3(1, -2, 0));
        //List<float> b = new List<float>();
        //b.Add(10);
        //b.Add(10);

        //Vector3 result = SimplexSolver.Solve(max, A, b);

   
        double[,] a = new double[,] { { 2, 0 }, { 0, 2 } };
        double[] b = new double[] { -6, -4 };
        double[] x0 = new double[] { 0, 1 };
        double[] bndl = new double[] { 0.0, 0.0 };
        double[] bndu = new double[] { 2.5, 2.5 };
        double[] x;
        alglib.minqpstate state;
        alglib.minqpreport rep;

        alglib.minqpcreate(2, out state);
        alglib.minqpsetquadraticterm(state, a);
        alglib.minqpsetlinearterm(state, b);
        alglib.minqpsetstartingpoint(state, x0);
        double[,] c = { { 1, 0, 2.5 }, {0,1,2.5}, { 1, 0, 0 }, {0, 1, 0} };
        int[] ct = { -1,-1, 1, 1 };
        alglib.minqpsetlc(state, c, ct);
        //alglib.minqpsetbc(state, bndl, bndu);
        alglib.minqpoptimize(state);
        alglib.minqpresults(state, out x, out rep);


        Debug.Log(rep.terminationtype); // EXPECTED: 4
        Debug.Log(alglib.ap.format(x, 2)); // EXPECTED: [2.5,2]
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * 5.0f;
        float y = Input.GetAxisRaw("Vertical") * 5.0f;

        testAgent.Velocity = new Vector3(x, 0.0f, y);

        for (int i = 0; i < agentManager.GetAgentCount(); ++i)
        {
            //if (Vector3.Distance(agentManager.GetAgent(i).Target, agentManager.GetAgent(i).Position) > 0.5f)
            //    agentManager.GetAgent(i).Velocity = agentManager.GetAgent(i).Target - agentManager.GetAgent(i).Position;
            //else
            //    agentManager.GetAgent(i).Velocity = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        agentManager.ResolveCollision(obstacles);
    }
}