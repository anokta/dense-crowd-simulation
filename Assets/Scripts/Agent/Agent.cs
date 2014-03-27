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
        get { return agentTransform.rigidbody.velocity; }
        set { agentTransform.rigidbody.velocity = value; }
    }

    void Start()
    {
        agentTransform = transform;
    }
}