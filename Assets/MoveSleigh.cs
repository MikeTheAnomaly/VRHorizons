using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveSleigh : MonoBehaviour
{
    public bool left;
    public bool right;
    public Vector2 MaxX = new Vector2(-100,100);
    public float moveSpeed = 20;
    public float PosX { get { return transform.position.x; } set { transform.position =  new Vector3( Mathf.Clamp(value, MaxX.x, MaxX.y), transform.position.y, transform.position.z); } }
    public InputActionAsset actionAsset;
    [SerializeField]
    private string controlerName;
    private InputAction move;

    public XRGrabInteractable leftGrab;
    public XRGrabInteractable rightGrab;
    
    // Start is called before the first frame update
    void Awake()
    {
        this.move = actionAsset.FindActionMap(controlerName).FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (leftGrab.isSelected && rightGrab.isSelected)
        {
            PosX += moveSpeed * Time.deltaTime * (Mathf.Clamp(leftGrab.transform.localPosition.z - rightGrab.transform.localPosition.z, -.2f,.2f) * 5);
            if (this.move.ReadValue<Vector2>().x < 0)
            {
                PosX += moveSpeed * Time.deltaTime;
            }
            else if (this.move.ReadValue<Vector2>().y > 0)
            {
                PosX -= moveSpeed * Time.deltaTime;
            }
        }
    }
}
