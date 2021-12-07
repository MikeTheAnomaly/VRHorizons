using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        //add alram or win status for pin
        foreach (var p in Pins)
        {
            p.OnHitUpdate.AddListener(normal =>
            {
                Debug.LogError(normal + " " + (((float)curRound / rounds) - .2f));
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
                        if (!Completed && (normal + .1) > (float)curRound / rounds)
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
        }
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
}
