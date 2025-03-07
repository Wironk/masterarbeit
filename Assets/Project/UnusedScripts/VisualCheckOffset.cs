using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCheckOffset : MonoBehaviour
{
    public Transform mainCamera;
    void Update()
    {
        float y = mainCamera.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(90, y, 0);

        transform.position = mainCamera.transform.position;
    }
}
