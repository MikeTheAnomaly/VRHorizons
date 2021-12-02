using System;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Puzzle[] Puzzles;
    public Trap[] Traps;

    private int curTrap = -1;
    private int completedPuzzles = 0;

    /// <summary>
    /// Unity Events
    /// </summary>
    public UnityEvent OnTrapActivate = new UnityEvent();
    public UnityEvent OnRoomFailed = new UnityEvent();
    public UnityEvent OnRoomPassed = new UnityEvent();

    void Start()
    {
        foreach (Puzzle puzzle in Puzzles)
        {
            puzzle.OnCompleted.AddListener(() =>
            {
                completedPuzzles++;
                CheckRoomStatus();
            });

            puzzle.OnFailed.AddListener(() =>
            {
                ActivateTrap();
            });
        }
    }

    /// <summary>
    /// Activates the next trap. Or Fails the room if not availble
    /// </summary>
    public void ActivateTrap()
    {
        curTrap++;
        if (curTrap < Traps.Length)
        {
            Traps[curTrap].Activate();
        }
        else
        {
            RoomFailed();
        }

        OnTrapActivate.Invoke();

    }

    /// <summary>
    /// Room is completed
    /// </summary>
    private void RoomPassed()
    {
        OnRoomPassed.Invoke();
    }

    /// <summary>
    /// Player failes the room
    /// </summary>
    private void RoomFailed()
    {
        OnRoomFailed.Invoke();
    }

    /// <summary>
    /// Checks to see the status of the room
    /// </summary>
    private void CheckRoomStatus()
    {
        if(completedPuzzles == Puzzles.Length)
        {
            RoomPassed();
        }
    }

}
