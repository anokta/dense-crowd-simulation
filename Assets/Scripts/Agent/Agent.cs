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

    Vector3 velocity;
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value.normalized * maxVelocity; }
    }

    float maxVelocity;
    public float MaxVelocity
    {
        get { return MaxVelocity; }
        set { maxVelocity = value; }
    }

    Vector3 target;
    public Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    void Awake()
    {
        agentTransform = transform;

        target = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
    }

    void Update()
    {
        agentTransform.position += Velocity * Time.deltaTime;
    }
}