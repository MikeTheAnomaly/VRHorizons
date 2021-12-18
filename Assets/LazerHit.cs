using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LazerHit : MonoBehaviour
{
    public UnityEvent OnPlayerHit = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("lazer hit" + other.tag);
        if(other.tag == "MainCamera")
        {
            OnPlayerHit.Invoke();
        }
    }
}
