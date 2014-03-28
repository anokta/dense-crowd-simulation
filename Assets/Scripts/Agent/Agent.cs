using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    Transform agentTransform;

    public Vector3 Position
    {
        get { return agentTransform.position; }
        set { agentTransform.position = value; }
    }

    public Vector3 Velocity
    {
        get;
        set;
    }

    public float minDistance;

    void Awake()
    {
        agentTransform = transform;

        minDistance = GetComponent<CapsuleCollider>().radius * 2.0f;
    }

    void Update()
    {
        agentTransform.position += Velocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        string type = collider.tag;

        if (type == "Agent")
        {
            resolveAgentCollision(collider.transform.position);
        }
        else if (type == "Obstacle")
        {
            resolveObstacleCollision(collider.transform.position);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        string type = collider.tag;

        if (type == "Agent")
        {
            resolveAgentCollision(collider.transform.position);
        }
        else if (type == "Obstacle")
        {
            resolveObstacleCollision(collider.transform.position);
        }
    }

    void resolveAgentCollision(Vector3 p)
    {
        float distance = Vector3.Distance(Position, p);

        Position += Vector3.Normalize(Position - p) * (minDistance - distance);
    }

    void resolveObstacleCollision(Vector3 p)
    {
        float distanceX = Mathf.Abs(Position.x - p.x);
        float distanceZ = Mathf.Abs(Position.z - p.z);

        Vector3 agentPosition = Position;
        if (distanceX > distanceZ)
            agentPosition.x += Mathf.Sign(agentPosition.x - p.x) * (minDistance - distanceX);
        else
            agentPosition.z += Mathf.Sign(agentPosition.z - p.z) * (minDistance - distanceZ);
        Position = agentPosition;
    }
}