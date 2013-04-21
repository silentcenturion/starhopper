using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    private Quaternion startRotation;
    private Vector3 targetRotation;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float startScale;
    private float targetScale;

    private StarMesh[] starMeshes;
    private Star _ActiveStar;

    private float rotationCooldown;
    private float currentTravelTime;
    private float targetTravelTime;

    private float zoomSpeed = 5;
    private float zoomDistanceMin = 0.1f;
    private float zoomDistanceMax = 200;
    private float smoothStartOrbitTime = 0;

    private float translationSpeed;

    public bool orbitActive;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float currentZoom = 3.5f;

    public GUIManager GuiManager;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        targetRotation.x = angles.y;
        targetRotation.y = angles.x;

        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }

    void LateUpdate()
    {
        Vector3 previousPos = transform.position;

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

            float speed = translationSpeed * Scaler.Scale;
            if (Input.GetKey(KeyCode.LeftShift))
                speed = translationSpeed * Scaler.Scale * 10;

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
            targetRotation.x = transform.eulerAngles.y;
            targetRotation.y = transform.eulerAngles.x;
            targetRotation.z = transform.eulerAngles.z;
        }
        else if (orbitActive)
        {
            float xDelta = Input.GetAxis("Mouse X") * xSpeed * 0.01f;
            float yDelta = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            if (xDelta != 0 || yDelta != 0)
            {
                //rotationCooldown = 0.5f;
                if (Input.GetKey(KeyCode.Space) ||
                    Input.GetKey(KeyCode.Mouse2))
                {
                    targetRotation.x += xDelta;
                    targetRotation.y -= yDelta;
                }
            }

            if (rotationCooldown > 0)
            {
                rotationCooldown -= Time.deltaTime;
            }
            else
            {
                smoothStartOrbitTime += Time.deltaTime;
                smoothStartOrbitTime = Mathf.Clamp01(smoothStartOrbitTime);

                targetRotation.x += 0.02f * smoothStartOrbitTime;
                targetRotation.y += 0.02f * smoothStartOrbitTime;
            }

            Quaternion rotation;

            //Debug.Log("currentTravelTime: " + currentTravelTime + ", targetTravelTime: " + targetTravelTime);
            if (currentTravelTime < targetTravelTime)
            {
                Vector3 direction = targetPosition * Scaler.Scale - Camera.mainCamera.transform.position;
                direction.Normalize();

                rotation = Quaternion.LookRotation(direction);
                targetRotation.x = rotation.eulerAngles.y;
                targetRotation.y = rotation.eulerAngles.x;
                targetRotation.z = rotation.eulerAngles.z;
            }
            else
            {
                rotation = Quaternion.Euler(targetRotation.y, targetRotation.x, targetRotation.z);
                startPosition = transform.position;
                startRotation = transform.rotation;
            }

            currentZoom = Mathf.Clamp(currentZoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, zoomDistanceMin, zoomDistanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -currentZoom);
            Vector3 position = rotation * negDistance + targetPosition * Scaler.Scale;

            bool wasTraveling = currentTravelTime < targetTravelTime;
            currentTravelTime += Time.deltaTime;
            bool isTraveling = currentTravelTime < targetTravelTime;

            if (wasTraveling && isTraveling == false)
            {
                if (_ActiveStar != null)
                {
                    Sun.CreateSolarSystem(_ActiveStar);
                }
            }

            float percent = 1;

            if (targetTravelTime > 0)
                percent = currentTravelTime / targetTravelTime;
            if (percent > 1)
                percent = 1;

            transform.position = Vector3.Slerp(startPosition, position, Mathf.SmoothStep(0, 1, percent));

            if (isTraveling)
                Scaler.Scale = Mathf.SmoothStep(startScale, targetScale, percent);

            if (targetTravelTime > 0)
                percent = currentTravelTime / (targetTravelTime * 0.5f);
            if (percent > 1)
                percent = 1;
            transform.rotation = Quaternion.Slerp(startRotation, rotation, Mathf.SmoothStep(0, 1, percent));
        }

        GuiManager.Speed = Vector3.Distance(previousPos, transform.position) * (1 / Scaler.Scale); 
    }

    public void DeactivateOrbit()
    {
        orbitActive = false;
        _ActiveStar = null;
    }

    public void SetMode(CameraMode mode)
    {
        OrbitLocation(StarPicker.Stars[0]);
        GuiManager.HideStarFocus();
        currentTravelTime = 0;
        targetTravelTime = 5f;

        switch (mode)
        {
            case CameraMode.Galaxy:
                zoomDistanceMax = 5000f;
                zoomSpeed = 5000f;
                currentZoom = 3000f;
                startScale = Scaler.Scale;
                targetScale = 3f;
                translationSpeed = 1f;
                break;
            case CameraMode.Solar:
                zoomDistanceMax = 200f;
                zoomSpeed = 5f;
                currentZoom = 3.5f;
                startScale = Scaler.Scale;
                targetScale = 100f;
                translationSpeed = 0.01f;
                break;
            case CameraMode.Free:
                zoomDistanceMax = 200f;
                zoomSpeed = 5f;
                currentZoom = 3.5f;
                startScale = Scaler.Scale;
                targetScale = 100f;
                translationSpeed = 0.01f;
                break;
            default:
                break;
        }
    }

    public void OrbitLocation(Star star)
    {
        currentTravelTime = 0;
        orbitActive = true;
        targetPosition = new Vector3(star.X, star.Y, star.Z);

        Vector3 direction = targetPosition * Scaler.Scale - Camera.mainCamera.transform.position;
        float lengthToTargetPos = direction.magnitude;
        targetTravelTime = 2f + lengthToTargetPos / 70f / Scaler.Scale;

        rotationCooldown = targetTravelTime + 5f;

        startPosition = transform.position;
        startRotation = transform.rotation;
        _ActiveStar = star;
        smoothStartOrbitTime = 0;
    }
}
