//Code taken and modified from http://wiki.unity3d.com/index.php?title=MouseOrbitImproved

using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform target;

    private float xAngle = 0.0f;
    private float yAngle = 0.0f;

    private float yMaxAngle = 45f;
    private float yMinAngle = -45f;

    private float distance = 8.0f;
    private float speed = 10.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xAngle = angles.y;
        yAngle = angles.x;
    }

    void LateUpdate()
    {
        if (target && Input.GetMouseButton(0))
        {
            xAngle += Input.GetAxis("Mouse X") * speed;
            yAngle -= Input.GetAxis("Mouse Y") * speed;
            yAngle = Mathf.Clamp(yAngle, yMinAngle, yMaxAngle);

            Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}