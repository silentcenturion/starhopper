using UnityEngine;
using System.Collections;

public class StarPicker : MonoBehaviour
{
    public static Star[] Stars;

    public GUIManager GuiManager;
    public OrbitCamera OrbitCamera;

    // Use this for initialization
    void Start()
    {
        Stars = LoadStars.Load();

        OrbitCamera.OrbitLocation(Stars[0]);
        GuiManager.SetStarFocus(Stars[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (GuiManager.IsMouseOverGui)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) ||
            Input.GetKeyDown(KeyCode.Mouse1))
        {
            OrbitCamera.DeactivateOrbit();
            GuiManager.HideStarFocus();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OrbitCamera.OrbitLocation(Stars[0]);
            GuiManager.SetStarFocus(Stars[0]);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Star closestStar;
            if (PickStar(out closestStar))
            {
                GuiManager.SetStarFocus(closestStar);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Star closestStar;
            if (PickStar(out closestStar))
            {
                OrbitCamera.OrbitLocation(closestStar);
                GuiManager.SetStarFocus(closestStar);
            }
        }
    }

    private bool PickStar(out Star closestStar)
    {
        float closestDistance;
        Vector3 camPos = Camera.mainCamera.transform.position;
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        closestDistance = float.MaxValue;
        closestStar = default(Star);

        foreach (Star star in Stars)
        {
            Vector3 starPos = new Vector3(star.X, star.Y, star.Z) * Scaler.Scale;
            float distance3D = Vector3.SqrMagnitude(camPos - starPos);
            if (distance3D < closestDistance &&
                distance3D < 200 * 200 * Scaler.Scale)
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
                }
            }
        }
        
        return closestDistance < float.MaxValue;
    }
}
