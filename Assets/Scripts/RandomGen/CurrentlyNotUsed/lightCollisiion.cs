using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCollisiion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("overlap at " + transform.position);
    }
}
