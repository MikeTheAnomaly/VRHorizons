using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LockPicking : Puzzle
{
    public LockPin[] Pins;
    public float percentChanceAlarmPin = .2f;

    private uint goodPinAmount;
    private uint currentCorrect = 0;
    private uint alarmPinAmount = 0;

    public uint rounds = 3;
    private uint curRound = 1;

    public bool debugColor = false;

    public GameObject pick;
    public float yBotOffset = .1f;

    public float failTollerance = .2f;

    private Vector3 pickStartPos;

    private void LateUpdate()
    {
        WSServer.SendToAllClients(GetJson());
    }
    private void Start()
    {
        pickStartPos = pick.transform.position;
        //add alram or win status for pin
        foreach (var p in Pins)
        {
            p.OnHitUpdate.AddListener(normal =>
            {
                //Debug.LogError(normal + " " + (((float)curRound / rounds) - .2f));
                if (!base.Failed)
                {
                    if (!p.AlarmPin)
                    {
                        if (!p.hasBeenHit && normal <= (float)curRound / rounds && normal >= ((float)curRound / rounds) - .2f)
                        {

                            p.hasBeenHit = true;
                            HitPin(p);
                        }
                    }
                    else
                    {
                        if (!Completed && (normal - failTollerance) > (float)curRound / rounds)
                        {
                            base.Fail();
                            if (debugColor)
                            {
                                p.gameObject.GetComponent<Renderer>().material = new Material(p.gameObject.GetComponent<Renderer>().material);
                                p.gameObject.GetComponent<Renderer>().material.color = Color.red;
                            }
                        }
                    }
                }
            });
        }
        UpdatePinStatus();
    }

    public void HitPin(LockPin pin)
    {
        currentCorrect++;
        pin.AlarmPin = true;
        if (debugColor)
        {
            pin.gameObject.GetComponent<Renderer>().material = new Material(pin.gameObject.GetComponent<Renderer>().material);
            pin.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        CheckStatus();
    }

    public void CheckStatus()
    {
        if(currentCorrect == goodPinAmount)
        {
            curRound++;
            if(curRound - 1  == rounds)
            {
                Complete();
            }
            else
            {
                UpdatePinStatus();
            }
            
        }
    }

    protected override void Reset()
    {
        base.Reset();
        foreach (var pin in Pins)
        {
            pin.Reset();
            pin.ResetPos();
        }
        UpdatePinStatus();
        //pick.transform.position = pickStartPos;
    }

    /// <summary>
    /// Takes a pin and randomly selects if it is a alarm pin or not
    /// </summary>
    /// <param name="pin"></param>
    /// <returns>True if it is a alarm pin</returns>
    private bool SetAlarmPinRandom(LockPin pin)
    {
        pin.AlarmPin = Random.Range(0f, 1f) <= percentChanceAlarmPin && (alarmPinAmount + 1) < Pins.Length;
        return pin.AlarmPin;
    }

    /// <summary>
    /// Gives new random values for the round of pins
    /// </summary>
    private void UpdatePinStatus()
    {
        currentCorrect = 0;
        goodPinAmount = 0;
        alarmPinAmount = 0;
        foreach (var pin in Pins)
        {
            pin.Reset();
            
            if (!SetAlarmPinRandom(pin)) 
            { 
                goodPinAmount++;
                if (debugColor)
                {
                    pin.gameObject.GetComponent<Renderer>().material = new Material(pin.gameObject.GetComponent<Renderer>().material);
                    pin.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
            else
            {
                alarmPinAmount++;
                if (debugColor)
                {
                    pin.gameObject.GetComponent<Renderer>().material = new Material(pin.gameObject.GetComponent<Renderer>().material);
                    pin.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }

        }
    }
    /// <summary>
    /// Gets the normlized position of the pick
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNormalizedPickPos()
    {
        //convert the pick to the local space of the picks so math stays right
        Vector3 pickLocalSpace = transform.InverseTransformPoint(pick.transform.position);
        float xSpace = Mathf.Clamp(pickLocalSpace.x, Pins[0].transform.localPosition.x, Pins[Pins.Length - 1].transform.localPosition.x);
        float ySpace = Mathf.Clamp(pick.transform.position.y, Pins[0].minHeight - yBotOffset, Pins[0].maxHeight);
        return new Vector3(xSpace.Normalize(Pins[0].transform.localPosition.x, Pins[Pins.Length - 1].transform.localPosition.x), ySpace.Normalize(Pins[0].minHeight - yBotOffset, Pins[0].maxHeight), 0);
    }

    public string GetJson()
    {
        PinSerilized[] pinSerilizeds = new PinSerilized[Pins.Length];
        for (int i = 0; i < Pins.Length; i++)
        {
            LockPin pin = Pins[i];
            pinSerilizeds[i] = new PinSerilized
            {
                NormalY = pin.NormalizedPinHeight,
                AlarmPin = pin.AlarmPin,
                Set = pin.hasBeenHit
            };
        }

        LockPickingSerialized lockPickingSerialized = new LockPickingSerialized
        {
            Pins = pinSerilizeds,
            Pick = new PickSerilized
            {
                NormalX = GetNormalizedPickPos().x,
                NormalY = GetNormalizedPickPos().y,
                RotZ = pick.transform.rotation.eulerAngles.z
            }
        };

        return JsonConvert.SerializeObject(lockPickingSerialized);
    }

    public class LockPickingSerialized
    {
        public PinSerilized[] Pins { get; set; }
        public PickSerilized Pick { get; set; }
    }

    public class PickSerilized
    {
        public float NormalX { get; set; }
        public float NormalY { get; set; }
        public float RotZ { get; set; }

    }

    public class PinSerilized
    {
        public float NormalY { get; set; }
        public bool AlarmPin { get; set; }
        public bool Set { get; set; }
    }
}
