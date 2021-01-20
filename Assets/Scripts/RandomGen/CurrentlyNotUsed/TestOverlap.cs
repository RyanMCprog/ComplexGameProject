using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOverlap : MonoBehaviour
{
    public bool Top = false;
    public bool Bottom = false;
    public bool North = false;
    public bool South = false;
    public bool West = false;
    public bool East = false;

    [HideInInspector]
    public bool AlreadySpawned = false;

    [HideInInspector]
    public GameObject TesterThatSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TestOverlap>())
        {
            if(other.GetComponent<TestOverlap>().AlreadySpawned)
            {
                Destroy(TesterThatSpawned);
            }
            else if(East)
            {
                Destroy(TesterThatSpawned);
            }
            else if(West && !other.GetComponent<TestOverlap>().East)
            {
                Destroy(TesterThatSpawned);
            }
            else if(South && (!other.GetComponent<TestOverlap>().East || !other.GetComponent<TestOverlap>().West))
            {
                Destroy(TesterThatSpawned);
            }
            else if(North && (other.GetComponent<TestOverlap>().Bottom || other.GetComponent<TestOverlap>().Top))
            {
                Destroy(TesterThatSpawned);
            }
            else if(Bottom && other.GetComponent<TestOverlap>().Top)
            {
                Destroy(TesterThatSpawned);
            }
            else
            {
                AlreadySpawned = true;
            }
        }
    }
}
