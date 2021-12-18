using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pick : MonoBehaviour
{
    public UnityEvent OnHitPin = new UnityEvent();
    public UnityEvent OnStopHitPin = new UnityEvent();

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "LockPickingPin")
        {
            other.gameObject.GetComponent<LockPin>().HitPinUpdate();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "LockPickingPin")
        {
            OnHitPin.Invoke();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "LockPickingPin")
        {
            OnStopHitPin.Invoke();
        }
    }

    private void LateUpdate()
    {
        
    }
}
