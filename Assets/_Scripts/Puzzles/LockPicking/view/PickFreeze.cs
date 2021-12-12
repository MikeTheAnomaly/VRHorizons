using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFreeze : MonoBehaviour
{
    private Vector3 angleLock;
    private Quaternion startRot;
    public bool RotLockX = false;
    public bool RotLockY = false;
    public bool RotLockZ = false;

    public Vector3 posLock;
    public float maxZ;
    public bool LockX = false;
    public bool lockZ = false;

    private float minY;
    private float minZ;

    private void Start()
    {
        angleLock = transform.rotation.eulerAngles;
        startRot = transform.rotation;
        posLock = transform.position;
        minY = transform.position.y;
        minZ = transform.position.z;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(RotLockX == true ? angleLock.x : transform.rotation.eulerAngles.x, RotLockY == true ? angleLock.y : transform.rotation.eulerAngles.y, RotLockZ == true ? angleLock.z : transform.rotation.eulerAngles.z));
        transform.position = new Vector3(LockX ? posLock.x : transform.position.x , Mathf.Clamp(transform.position.y, minY, float.PositiveInfinity), lockZ ? posLock.z : Mathf.Clamp(transform.position.z,minZ,minZ + maxZ));
    }

    public void ResetPos()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = posLock;
        transform.rotation = startRot;
    }
}
