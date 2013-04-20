using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour
{
    public static float Scale = 100f;

    void Update()
    {
        transform.localScale = Vector3.one * Scale;
    }
}
