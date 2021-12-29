using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{

    public bool inSafeArea;
    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnCollisionStay(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        inSafeArea = true;
        Debug.Log("enter the Safe area");
    }
    private void OnTriggerStay(Collider other)
    {
        inSafeArea = true;
        Debug.Log("in Safe area");
    }

    private void OnTriggerExit(Collider other)
    {
        inSafeArea = false;
        Debug.Log("out of the Safe area");
    }
}
