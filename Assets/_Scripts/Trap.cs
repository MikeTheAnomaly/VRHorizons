using System;
using UnityEngine;
using UnityEngine.Events;

public class Trap : Puzzle
{
    public UnityEvent OnTrapActive = new UnityEvent();
    public UnityEvent OnTrapInactive = new UnityEvent();
    public bool IsActive;

    public virtual void Activate()
    {
        OnTrapActive.Invoke();
        IsActive = true;
    }

    protected override void Complete()
    {
        base.Complete();
        IsActive = false;
        OnTrapInactive.Invoke();
    }



}
