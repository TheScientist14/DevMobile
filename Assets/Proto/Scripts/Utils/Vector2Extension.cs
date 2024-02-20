using UnityEngine;

public static class Vector2Extension
{
    /// <summary>
    /// Rotates the vector by the given angle
    /// </summary>
    /// <param name="v"></param>
    /// <param name="degrees">angle in degree</param>
    /// <returns></returns>
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }
    
    /// <summary>
    /// Return the vector of a quadratic bezier curve
    /// </summary>
    /// <param name="a">Starting point</param>
    /// <param name="b">Curving point</param>
    /// <param name="c">End point</param>
    /// <param name="t">Transition value</param>
    /// <returns>Curve location at giver value</returns>
    public static Vector2 QuadraticCurve(this Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p0 = Vector2.Lerp(a, b, t);
        Vector2 p1 = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(p0, p1, t);
    }

    /// <summary>
    /// Returns an underestimation of the length of a quadratic bezier curve
    /// </summary>
    /// <param name="a">Starting point</param>
    /// <param name="b">Curving point</param>
    /// <param name="c">End point</param>
    /// <param name="Precision">Precision of the length calculus (Costful)</param>
    /// <returns></returns>
    public static float QuadraticCurveLength(this Vector2 a, Vector2 b, Vector2 c, int Precision = 7)
    {
        float Length = 0;
        Vector2 OldPoint = a;
        for (int i = 0; i <= Precision; i++)
        {
            Vector2 NewPoint=a.QuadraticCurve(b, c, (float)i / Precision);
            Length+=Vector2.Distance(OldPoint, NewPoint);
            OldPoint=NewPoint;
        }
        return Length;
    }

    public static Vector3 ToVector3(this Vector2 a, float z=0)
    {
        return new Vector3(a.x, a.y, z);
    }

}

