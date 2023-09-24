using OpenTK.Mathematics;

namespace OpenTKTest.Source.Utils;

public static class RadsExtensions
{
    public static Vector3 DegreesToRads(this Vector3 degrees)
    {
        return new Vector3(
            degrees.X * Single.Pi / 180f,
            degrees.Y * Single.Pi / 180f,
            degrees.Z * Single.Pi / 180f
        );
    }
}