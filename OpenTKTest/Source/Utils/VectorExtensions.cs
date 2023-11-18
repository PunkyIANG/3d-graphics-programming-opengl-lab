using OpenTK.Mathematics;

namespace OpenTKTest.Source.Utils;

public static class VectorExtensions
{
    public static Vector2 Scale(this Vector2 vec, float t) => new Vector2(
        vec.X * t,
        vec.Y * t
    );
    
    public static Vector3 Scale(this Vector3 vec, float t) => new Vector3(
        vec.X * t,
        vec.Y * t,
        vec.Z * t
    );

}