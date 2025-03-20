using System;
using UnityEngine;

public class TestS : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError(other.name);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError(other.name);

    }
}
