using UnityEngine;
using System.Collections;

public class StarPicker : MonoBehaviour
{
    Star[] stars;

    OrbitCamera orbitCamera;

    public static Star SelectedStar;

    // Use this for initialization
    void Awake()
    {
        orbitCamera = Camera.mainCamera.GetComponent<OrbitCamera>();

        stars = LoadStars.Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 camPos = Camera.mainCamera.transform.position;
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            float closestDistance = float.MaxValue;
            Star closestStar = default(Star);
            Vector3 closestStarPosition = Vector3.zero;

            foreach (Star star in stars)
            {
                Vector3 starPos = new Vector3(star.X, star.Y, star.Z) * Scaler.Scale;
                float distance3D = Vector3.SqrMagnitude(camPos - starPos);
                if (distance3D < closestDistance &&
                    distance3D < 400 * 400)
                {
                    Vector3 screenPos3D = Camera.mainCamera.WorldToScreenPoint(starPos);
                    screenPos3D.y = Screen.height - screenPos3D.y;
                    if (screenPos3D.z < 0)
                        continue;

                    Vector2 screenPos = new Vector2(screenPos3D.x, screenPos3D.y);
                    if ((screenPos - mousePos).sqrMagnitude < 20 * 20)
                    {
                        closestDistance = distance3D;
                        closestStar = star;
                        closestStarPosition = starPos;
                    }
                }
            }

            if (closestDistance < float.MaxValue)
            {
                orbitCamera.OrbitLocation(closestStarPosition);
                SelectedStar = closestStar;
            }
        }
    }
}
