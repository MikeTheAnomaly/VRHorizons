using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public bool Completed { get; protected set; }
    public UnityEvent OnCompleted = new UnityEvent();

    /// <summary>
    /// Completes the puzzle and invokes the on complete;
    /// </summary>
    protected virtual void Complete()
    {
        OnCompleted.Invoke();
    }

    /// <summary>
    /// Resets the puzzle to noncompleted status
    /// </summary>
    protected virtual void Reset()
    {
        Completed = false;
    }
}
