using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "LockPickingPin")
        {
            other.gameObject.GetComponent<LockPin>().HitPinUpdate();
        }
    }
}
