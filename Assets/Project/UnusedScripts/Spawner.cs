using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void SpawnWhiteboard()
    {
        GameObject newWhiteboard = (GameObject)Instantiate(Resources.Load("Whiteboard"));
    }
}
