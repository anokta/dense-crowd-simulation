using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
    public Vector3 speed;
    public float minY, maxY;

    float x, y;
    float distance;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        distance = transform.position.y;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * speed.x;
            y -= Input.GetAxis("Mouse Y") * speed.y;

            y = ClampAngle(y, minY, maxY);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            
            transform.rotation = rotation;
            transform.position = rotation * new Vector3(0.0f, 0.0f, -distance);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance -= speed.z * Input.GetAxis("Mouse ScrollWheel");

            transform.position = Quaternion.Euler(y, x, 0) * new Vector3(0.0f, 0.0f, -distance);
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)   angle += 360;
        else if (angle > 360)   angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}