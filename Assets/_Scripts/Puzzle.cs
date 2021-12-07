using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public bool Completed { get; protected set; }
    public bool Failed { get; protected set; }
    public UnityEvent OnCompleted = new UnityEvent();
    public UnityEvent OnFailed = new UnityEvent();

    /// <summary>
    /// Completes the puzzle and invokes the on complete;
    /// </summary>
    protected virtual void Complete()
    {
        OnCompleted.Invoke();
        Completed = true;
    }

    protected virtual void Fail()
    {
        OnFailed.Invoke();
        Failed = true;
    }

    /// <summary>
    /// Resets the puzzle to noncompleted status
    /// </summary>
    protected virtual void Reset()
    {
        Completed = false;
        Failed = false;
    }
}
