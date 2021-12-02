using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockPin : MonoBehaviour
{
    public bool AlarmPin = false;

    private bool hasBeenHit = false;

    public UnityEvent OnHit = new UnityEvent();

    public void HitPin()
    {
        if (!hasBeenHit)
        {
            OnHit.Invoke();
            hasBeenHit = true;
        }
    }

    public void Reset()
    {
        hasBeenHit = false;
    }

}
