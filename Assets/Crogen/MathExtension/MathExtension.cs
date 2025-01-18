using UnityEngine;

public static class MathExtension
{
    public static float RotateClamp(float value, float min, float max)
    {
        float angle = value > 180f ? value - 360f : value;

        angle = Mathf.Clamp(value, min, max);

        return angle;
    }

    public static Vector3 VectorClamp(Vector3 value, Vector3 min, Vector3 max)
    {
        for (short i = 0; i < 3; ++i)
        {
            if (value[i] > max[i])
            {
                value[i] = max[i];
            }
            if (value[i] < min[i])
            {
                value[i] = min[i];
            }
        }
        return value;
    }
    
    public static float PowerByTwo(float x) 
    {
        return x * x;
    }
    
    public static float Remap(float value, float in1, float in2, float out1, float out2)
    {
        return out1 + (value - in1) * (out2 - out1) / (in2 - in1);
    }

    public static Vector3 ClampMagnitude(Vector3 vec, float min, float max)
    {
        if (vec.magnitude < min)
            vec = vec.normalized * min;
        else if (vec.magnitude > max)
            vec = vec.normalized * max;
        return vec;
    }
}
