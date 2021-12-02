using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPicking : Puzzle
{
    public LockPin[] Pins;
    public float percentChanceAlarmPin = .2f;

    private int goodPinAmount;
    private int currentCorrect = 0;

    private void Start()
    {
        foreach (var pin in Pins)
        {
            SetAlarmPinRandom(pin);

            if (!pin.AlarmPin)
            {
                pin.OnHit.AddListener(() =>
                {
                    HitPin(pin);
                });
                goodPinAmount++;
            }
            else
            {
                pin.OnHit.AddListener(() =>
                {
                    base.Fail();
                });

            }
        }
    }

    public void HitPin(LockPin pin)
    {
        currentCorrect++;
    }

    public void CheckStatus()
    {
        if(currentCorrect == goodPinAmount)
        {
            Complete();
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

    private void SetAlarmPinRandom(LockPin pin)
    {
        pin.AlarmPin = Random.Range(0f, 1f) <= percentChanceAlarmPin;
    }
}
