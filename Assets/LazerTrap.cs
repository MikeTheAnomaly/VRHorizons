using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrap : Trap
{
    // Start is called before the first frame update
    public LazerHit[] lazers;
    void Start()
    {
        foreach (var lazer in lazers)
        {
            lazer.OnPlayerHit.AddListener(() =>
            {
                Fail();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainCamera")
        {
            Fail();
        }
    }
}
