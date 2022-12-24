using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadRope : MonoBehaviour
{
    public GameObject[] points;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3[] points = new Vector3[this.points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = this.points[i].transform.position;
        }
        lineRenderer.SetPositions(points);
    }
}
