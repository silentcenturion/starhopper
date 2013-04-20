using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour
{
    public static float Scale = 150f;

    void Update()
    {
        transform.localScale = Vector3.one * Scale;
    }
}
