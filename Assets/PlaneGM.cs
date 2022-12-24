using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlaneGM : MonoBehaviour
{
    public MoveSleigh sleigh;
    [SerializeField]
    private int health = 6;
    public int Health { get { return health; } set { if (this.health == value) return; OnHealthChange.Invoke(value); this.txtHealth.text = "Reindeer: " + value; this.health = value; } }
    public UnityEvent<int> OnHealthChange = new UnityEvent<int>();
    public TMP_Text txtHealth;

    public Vector2 SpawnAreaZ = new Vector2(-900, -400);
    public Vector2 MaxX = new Vector2(-50, 50);
    public int poolAmount = 10;
    Queue<PlaneInstance> objectPool = new Queue<PlaneInstance>();
    List<PlaneInstance> planeInstances = new List<PlaneInstance>();
    public PlaneInstance plane;
    

    // Start is called before the first frame update
    void Awake()
    {
        this.MaxX = sleigh.MaxX;
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject refrence = GameObject.Instantiate(plane.gameObject);
            refrence.name = "plane " + i;
            var instance = refrence.GetComponent<PlaneInstance>();
            instance.spawnAreaZ = SpawnAreaZ.y;
            instance.GM = this;
            planeInstances.Add(instance);
            objectPool.Enqueue(instance);
            refrence.SetActive(false);
        }
    }

    private void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        WSServer.SendToAllClients(JsonConvert.SerializeObject(GetJsonObject()));
    }

    public void Spawn()
    {
        PlaneInstance planeInstance = objectPool.Dequeue();
        planeInstance.transform.position = new Vector3(Random.Range(MaxX.x + planeInstance.PlaneWidth, MaxX.y - planeInstance.PlaneWidth), planeInstance.transform.position.y, Random.Range(SpawnAreaZ.x, SpawnAreaZ.y));
        planeInstance.gameObject.SetActive(true);

    }

    public void ReQueue(PlaneInstance plane)
    {
        objectPool.Enqueue(plane);
        plane.gameObject.SetActive(false);

        Debug.Log(JsonConvert.SerializeObject(GetJsonObject()));
    }

    public SleighGameJson GetJsonObject()
    {
        List<PlanePosJson> list = new List<PlanePosJson>();
        foreach (var instance in planeInstances)
        {
            PlanePosJson pos = new PlanePosJson
            {
                name = instance.gameObject.name,
                posXNormal = (instance.transform.position.x - sleigh.MaxX.x) / (sleigh.MaxX.y - sleigh.MaxX.x),
                posZNormal = (instance.transform.position.z - -1000) / (sleigh.transform.position.z - -1000),
                active = instance.gameObject.activeInHierarchy
            };
            list.Add(pos);
        }

        return new SleighGameJson
        {
            health = this.Health,
            sleighPosNorm = (this.sleigh.transform.position.x - sleigh.MaxX.x) / (sleigh.MaxX.y - sleigh.MaxX.x),
            planes = list.ToArray()
        };
    }
}

public class SleighGameJson
{
    public int health;
    public float sleighPosNorm;
    public PlanePosJson[] planes;

}

public class PlanePosJson
{
    public string name;
    public float posXNormal;
    public float posZNormal;
    public bool active;
}
