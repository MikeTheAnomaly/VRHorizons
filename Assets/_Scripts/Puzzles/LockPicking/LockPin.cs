using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class LockPin : MonoBehaviour
{
    public bool AlarmPin = false;

    public bool hasBeenHit = false;

    private Rigidbody rb;
    public float minHeight { get; private set; }
    public float maxHeight;

    public float NormalizedPinHeight { get { return (transform.position.y - minHeight) / (maxHeight - minHeight); } }

    public UnityEvent<float> OnHitUpdate = new UnityEvent<float>();

    private void Start()
    {
        minHeight = transform.position.y;
        maxHeight += transform.position.y;

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
    }

    private void LateUpdate()
    {
        //constrain the position of the peg on the axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, maxHeight), transform.position.z);
    }

    public void HitPinUpdate()
    {
            OnHitUpdate.Invoke(NormalizedPinHeight);
    }

    public void Reset()
    {
        hasBeenHit = false;
    }

    public void ResetPos()
    {
        transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
    }

}
