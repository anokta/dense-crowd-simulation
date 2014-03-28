using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    Transform obstacleTransform;

    public Vector3 Position
    {
        get { return obstacleTransform.position; }
        set { obstacleTransform.position = value; }
    }

    void Awake()
    {
        obstacleTransform = transform;
    }
}