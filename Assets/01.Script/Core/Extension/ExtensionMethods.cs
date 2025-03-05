using System;
using UnityEngine;

public static class ExtensionMethods
{

}

public static class MathEx
{
    public static Vector2 Bezier(float t, params Vector2[] positions)
    {
        if (positions.Length == 0) 
            return Vector2.zero;
        if (positions.Length == 1) 
            return positions[0];

        Vector2[] nextPositions = new Vector2[positions.Length - 1];

        for (int i = 0; i < nextPositions.Length; i++)
        {
            nextPositions[i] = Vector2.Lerp(positions[i], positions[i + 1], t);
        }

        return Bezier(t, nextPositions);
    }
}