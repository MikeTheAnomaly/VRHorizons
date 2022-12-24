using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInstance : MonoBehaviour
{
    public PlaneGM GM;
    public float speed;
    public float maxZ = 100;
    public float PlaneWidth = 10;

    public float spawnAreaZ = -399;
    public bool spawnTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnTrigger && this.transform.position.z > spawnAreaZ)
        {
            GM.Spawn();
            spawnTrigger = true;
        }

        if (this.transform.position.z < maxZ)
        {
            this.transform.position += new Vector3(0,0, maxZ * Time.deltaTime);
        }
        else
        {
            spawnTrigger = false;
            GM.ReQueue(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sleigh")
        {
            GM.Health--;
        }
    }
}
