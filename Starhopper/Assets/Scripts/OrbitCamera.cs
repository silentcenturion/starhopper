using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;

    private StarMesh[] starMeshes;

    private float rotationCooldown;
    private float currentTravelTime;

    public bool orbitActive;

    public Vector3 targetPosition;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float zoomSpeed = 5;
    public float zoomDistanceMin = 0.5f;
    public float zoomDistanceMax = 200;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
            Scaler.Scale += 0.1f;

        if (Input.GetKey(KeyCode.KeypadMinus))
            Scaler.Scale -= 0.1f;


        if (Input.GetKeyDown(KeyCode.Return))
            OrbitLocation(Vector3.zero);

        if (Input.GetKey(KeyCode.Mouse1) ||
            Input.GetKey(KeyCode.Mouse2) ||
            Input.GetKey(KeyCode.Space))
        {
            orbitActive = false;

            float rotationX = Input.GetAxis("Mouse X") * 1f;
            transform.Rotate(0, rotationX, 0);

            float rotationY = Input.GetAxis("Mouse Y") * 1f;
            transform.Rotate(-rotationY, 0, 0);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float rotationZ = Input.GetAxis("Mouse X") * 1f;
                transform.Rotate(0, 0, rotationZ);
            }

            float speed = 0.1f;
            if (Input.GetKey(KeyCode.LeftShift))
                speed = 1f;

            if (Input.GetKey(KeyCode.A))
                transform.Translate(-speed, 0, 0);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(speed, 0, 0);

            if (Input.GetKey(KeyCode.S))
                transform.Translate(0, 0, -speed);
            if (Input.GetKey(KeyCode.W))
                transform.Translate(0, 0, speed);

            if (Input.GetKey(KeyCode.Q))
                transform.Translate(0, -speed, 0);
            if (Input.GetKey(KeyCode.E))
                transform.Translate(0, speed, 0);

            targetPosition = transform.position + transform.forward * distance;
            x = transform.eulerAngles.y;
            y = transform.eulerAngles.x;
            z = transform.eulerAngles.z;
        }
        else if (orbitActive)
        {
            float xDelta = Input.GetAxis("Mouse X") * xSpeed * 0.01f;
            float yDelta = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            if (xDelta != 0 || yDelta != 0)
            {
                rotationCooldown = 3;
                x += xDelta;
                y -= yDelta;
            }

            if (rotationCooldown > 0)
            {
                rotationCooldown -= Time.deltaTime;
            }
            else
            {
                x += 0.02f;
                y += 0.02f;
            }

            Quaternion rotation = Quaternion.Euler(y, x, z);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, zoomDistanceMin, zoomDistanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + targetPosition;

            transform.rotation = rotation;

            currentTravelTime += Time.deltaTime;

            float percent = currentTravelTime / 2f;
            if (percent > 1)
                percent = 1;
            transform.position = Vector3.Slerp(transform.position, position, percent);
        }
    }

    public void OrbitLocation(Vector3 position)
    {
        currentTravelTime = 0;
        targetPosition = position;
        orbitActive = true;
    }
}
