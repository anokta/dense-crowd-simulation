using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour
{
    public Texture2D mapTexture;

    public GameObject groundPrefab, obstaclePrefab;

    bool[,] map;

    static MapLoader instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadMapIntoScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadMapIntoScene()
    {
        map = new bool[mapTexture.height, mapTexture.width];

        Transform entityContainer = GameObject.Find("20 Entities").transform;

        // Instantiate the ground
        GameObject ground = GameObject.Instantiate(groundPrefab) as GameObject;
        ground.transform.localScale = new Vector3(mapTexture.width / 10.0f, groundPrefab.transform.localScale.y, mapTexture.height / 10.0f);
        ground.transform.parent = entityContainer;

        // Instantiate the obstacle
        Transform obstacleContainer = new GameObject("Obstacles").transform;
        obstacleContainer.parent = entityContainer;

        for (int y = 0; y < mapTexture.height; ++y)
        {
            for (int x = 0; x < mapTexture.width; ++x)
            {
                map[y, x] = (mapTexture.GetPixel(x, y) == Color.black);
                if (map[y, x])
                {
                    GameObject obstacle = GameObject.Instantiate(obstaclePrefab, new Vector3(x - (mapTexture.width - 1) / 2.0f, obstaclePrefab.transform.position.y, -y + (mapTexture.height - 1) / 2.0f), Quaternion.identity) as GameObject;
                    obstacle.transform.parent = obstacleContainer;
                }
            }
        }
    }

    public static bool MapGrid(int x, int y)
    {
        return instance.map[y, x];
    }
}