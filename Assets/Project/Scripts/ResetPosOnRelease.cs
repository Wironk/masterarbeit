using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosOnRelease : MonoBehaviour
{

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    void Start()
    {
        initialLocalPos = transform.position;
        initialLocalRot = transform.rotation;
    }

    public void OnReleased()
    {
        transform.position = initialLocalPos;
        transform.rotation = initialLocalRot;
    }
}
