using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour
{
    public GameObject groundPrefab, obstaclePrefab;

    bool[,] map;
    int width, height;

    public void LoadMapIntoScene(Texture2D mapTexture)
    {
        width = mapTexture.width;
        height = mapTexture.height;

        map = new bool[height, width];

        Transform entityContainer = GameObject.Find("20 Entities").transform;

        // Instantiate the ground
        GameObject ground = GameObject.Instantiate(groundPrefab) as GameObject;
        ground.transform.localScale = new Vector3(width / 10.0f, groundPrefab.transform.localScale.y, height / 10.0f);
        ground.transform.parent = entityContainer;

        // Instantiate the obstacle
        Transform obstacleContainer = new GameObject("Obstacles").transform;
        obstacleContainer.parent = entityContainer;

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                map[y, x] = (mapTexture.GetPixel(x, y) == Color.black);
                if (map[y, x])
                {
                    GameObject obstacle = GameObject.Instantiate(obstaclePrefab, new Vector3(x - (width - 1) / 2.0f, obstaclePrefab.transform.position.y, -y + (height - 1) / 2.0f), Quaternion.identity) as GameObject;
                    obstacle.transform.parent = obstacleContainer;
                }
            }
        }
    }
}