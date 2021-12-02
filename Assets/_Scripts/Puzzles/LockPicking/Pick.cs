using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LockPickingPin")
        {
            other.GetComponent<LockPin>().HitPin();
        }
    }
}
