using System;
using UnityEngine;

public static class ExtensionMethods
{

}

public static class MathEx
{
    #region Ease
    public static float OutSine(float x)
        => Mathf.Sin((x* Mathf.PI) / 2f);
    public static float OutQuad(float x)
        => 1f - (1f - x) * (1f - x);
    public static float OutCubic(float x)
        => 1f - Mathf.Pow(1f - x, 3f);
    public static float OutCirc(float x)
        => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    #endregion

    public static Vector2 Bezier(float t, params Vector2[] positions)
    {
        if (positions.Length == 0) 
            return Vector2.zero;
        if (positions.Length == 1) 
            return positions[0];
        if(positions.Length == 2)
            return Vector2.Lerp(positions[0], positions[1], t);

        Vector2[] nextPositions = new Vector2[positions.Length - 1];

        for (int i = 0; i < nextPositions.Length; i++)
        {
            nextPositions[i] = Vector2.Lerp(positions[i], positions[i + 1], t);
        }

        return Bezier(t, nextPositions);
    }
}