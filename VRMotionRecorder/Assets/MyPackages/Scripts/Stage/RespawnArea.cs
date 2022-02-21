using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    private static readonly string PLAYER = "Player";

    private void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.transform.root.gameObject;

        if( true == parent.CompareTag(PLAYER))
        {
            var spawn_pos = CV.zero;

            var points = FindObjectsOfType<SpawnPos>();
            if (0 < points.Length)
            {
                int random = UnityEngine.Random.Range(0, points.Length);
                spawn_pos = points[random].transform.position;
            }

            parent.transform.position = spawn_pos;
        }
    }
} 
