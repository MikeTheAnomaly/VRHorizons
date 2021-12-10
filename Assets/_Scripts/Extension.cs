using Fleck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static float Normalize(this float num, float min, float max)
    {
        return (num - min) / (max - min);
    }
}
