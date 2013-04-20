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
    private float targetTravelTime;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool orbitActive;

    public Vector3 targetPosition;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float currentZoom = 50.0f;
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

        if (Input.GetKey(KeyCode.Mouse1))
        {
            float rotationX = Input.GetAxis("Mouse X") * 1f;
            transform.Rotate(0, rotationX, 0);

            float rotationY = Input.GetAxis("Mouse Y") * 1f;
            transform.Rotate(-rotationY, 0, 0);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                float rotationZ = Input.GetAxis("Mouse X") * 1f;
                transform.Rotate(0, 0, rotationZ);
            }

            float speed = 0.01f * Scaler.Scale;
            if (Input.GetKey(KeyCode.LeftShift))
                speed = 0.1f * Scaler.Scale;

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

            targetPosition = transform.position + transform.forward * currentZoom;
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
                rotationCooldown = 0.5f;
                if (Input.GetKey(KeyCode.Space) ||
                    Input.GetKey(KeyCode.Mouse2))
                {
                    x += xDelta;
                    y -= yDelta;
                }
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

            Quaternion rotation;

            //Debug.Log("currentTravelTime: " + currentTravelTime + ", targetTravelTime: " + targetTravelTime);
            if (currentTravelTime < targetTravelTime)
            {
                Vector3 direction = targetPosition * Scaler.Scale - Camera.mainCamera.transform.position;
                direction.Normalize();

                rotation = Quaternion.LookRotation(direction);
                x = rotation.eulerAngles.y;
                y = rotation.eulerAngles.x;
                z = rotation.eulerAngles.z;
            }
            else
            {
                rotation = Quaternion.Euler(y, x, z);
                startPosition = transform.position;
                startRotation = transform.rotation;
            }

            currentZoom = Mathf.Clamp(currentZoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, zoomDistanceMin, zoomDistanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -currentZoom);
            Vector3 position = rotation * negDistance + targetPosition * Scaler.Scale;

            currentTravelTime += Time.deltaTime;

            float percent = 1;

            if (targetTravelTime > 0)
                percent = currentTravelTime / targetTravelTime;
            if (percent > 1)
                percent = 1;
            transform.position = Vector3.Slerp(startPosition, position, Mathf.SmoothStep(0, 1, percent));

            if (targetTravelTime > 0)
                percent = currentTravelTime / (targetTravelTime * 0.5f);
            if (percent > 1)
                percent = 1;
            transform.rotation = Quaternion.Slerp(startRotation, rotation, Mathf.SmoothStep(0, 1, percent));
        }
    }

    public void DeactivateOrbit()
    {
        orbitActive = false;
    }

    public void OrbitLocation(Star star)
    {
        currentTravelTime = 0;
        orbitActive = true;
        targetPosition = new Vector3(star.X, star.Y, star.Z);

        Vector3 direction = targetPosition * Scaler.Scale - Camera.mainCamera.transform.position;
        float lengthToTargetPos = direction.magnitude;
        targetTravelTime = lengthToTargetPos / 2 / Scaler.Scale;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}
