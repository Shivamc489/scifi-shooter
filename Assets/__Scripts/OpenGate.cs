using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    private bool hasOpened = false;
    public Animator[] animators;
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Truigger entered : "+other.name);
        if(hasOpened)
        {
            return;
        }
        if(other.tag == "floor")
        {
            Debug.Log("floor trigerred");
            hasOpened = true;
            foreach (var x in animators)
            {
                x.Play("open");
            }
        }
    }
}
