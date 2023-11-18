using System.Text;
using OpenTK.Mathematics;
using OpenTKTest.Utils;

namespace OpenTKTest.Source.Utils;

public static class Prefabs
{
    public static Model GetQuad() => ModelLoader.LoadBasicModel("Models/Quad/quad.obj");

    public static Model GetTripleArrow() =>
        ModelLoader.LoadTexturedModel("Models/ThreeArrows/threearrows.obj", "Models/ThreeArrows/threearrows.mtl");

    public static Model GetArrowX() => ModelLoader.LoadGradientModel(
        "Models/Arrow/arrow.obj", 
        Vector3.UnitX.Scale(-0.5f),
        Vector3.UnitX.Scale(0.5f),
        Vector4.Zero,
        new Vector4(1, 0, 0, 0)
        );
    public static Model GetArrowY() => ModelLoader.LoadGradientModel(
        "Models/Arrow/arrowy.obj",
        Vector3.UnitY.Scale(-0.5f),
        Vector3.UnitY.Scale(0.5f),
        Vector4.Zero,
        new Vector4(0, 1, 0, 0)
        );
    public static Model GetArrowZ() => ModelLoader.LoadGradientModel(
        "Models/Arrow/arrowz.obj",
        Vector3.UnitZ.Scale(0.5f),
        Vector3.UnitZ.Scale(-0.5f),
        Vector4.Zero,
        new Vector4(0, 0, 1, 0)
        );
    public static Model GetSphere() => ModelLoader.LoadBasicModel("Models/Arrow/sphere.obj");

    public static Model GetTexturedQuad() =>
        ModelLoader.LoadTexturedModel("Models/Quad/quad.obj", "Models/Quad/quad.mtl");

    public static Model GetGradientQuad() => ModelLoader.LoadGradientQuad();

    public static Model GetProceduralModel(uint rows, uint columns)
    {
        var tempVertices = new Vector3[rows + 1, columns + 1, 5];
        for (int i = 0; i <= rows; i++)
        for (int j = 0; j <= columns; j++)
        {
            var radius = (float)i / rows;

            //lower quarter
            tempVertices[i, j, 0] = new Vector3((radius + PosCos(0.5f) * radius) * 2,
                (-radius + PosSin(0.5f) * radius) * 2, radius * 2);

            //circle stuff
            tempVertices[i, j, 1] = new Vector3((-radius + PosCos(0.5f)) * 1.5f * radius,
                radius + PosSin(0.5f) * radius, radius * 2);
            tempVertices[i, j, 2] = new Vector3((-radius + PosCos(0f)) * 1.5f * radius, radius + PosSin(0f) * radius,
                radius * 2);
            tempVertices[i, j, 3] = new Vector3((-radius + PosCos(1f)) * 1.5f * radius, radius + PosSin(1f) * radius,
                radius * 2);
            tempVertices[i, j, 4] = new Vector3((-radius + PosCos(1.5f)) * 1.5f * radius,
                radius + PosSin(1.5f) * radius, radius * 2);

            float PosCos(float offset) => (float)Math.Cos(Math.PI * (offset + 0.5 * ((float)j / columns)));
            float PosSin(float offset) => (float)Math.Sin(Math.PI * (offset + 0.5 * ((float)j / columns)));
        }

        // flatten the array
        var vertices = new Vector3[(rows + 1) * (columns + 1) * 5];

        for (int k = 0; k <= 4; k++)
        for (int i = 0; i <= rows; i++)
        for (int j = 0; j <= columns; j++)
            vertices[ToIndex(k, i, j)] = tempVertices[i, j, k];

        var positions = new float[vertices.Length * 3 * 2];

        foreach (var (vertex, index) in vertices.WithIndex())
        {
            positions[3 * index] = vertex.X / 3;
            positions[3 * index + 1] = vertex.Y / 3;
            positions[3 * index + 2] = vertex.Z / 3;

            positions[positions.Length / 2 + 3 * index] = vertex.X / -3;
            positions[positions.Length / 2 + 3 * index + 1] = vertex.Y / -3;
            positions[positions.Length / 2 + 3 * index + 2] = vertex.Z / -3;
        }

        var indices = new List<uint>();

        for (int k = 0; k < 5; k++)
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        {
            indices.Add(ToIndex(k, i, j));
            indices.Add(ToIndex(k, i, j + 1));
            indices.Add(ToIndex(k, i + 1, j));
            indices.Add(ToIndex(k, i, j + 1));
            indices.Add(ToIndex(k, i + 1, j));
            indices.Add(ToIndex(k, i + 1, j + 1));

            uint offset = (uint)vertices.Length;

            indices.Add(offset + ToIndex(k, i, j));
            indices.Add(offset + ToIndex(k, i, j + 1));
            indices.Add(offset + ToIndex(k, i + 1, j));
            indices.Add(offset + ToIndex(k, i, j + 1));
            indices.Add(offset + ToIndex(k, i + 1, j));
            indices.Add(offset + ToIndex(k, i + 1, j + 1));
        }

        // var stringBuilder = new StringBuilder();
        //
        // foreach (var vertex in vertices) 
        //     stringBuilder.Append($"v {vertex.X} {vertex.Y} {vertex.Z} \n");
        //
        // for (int i = 0; i < indices.Count / 3; i++)
        //     stringBuilder.Append($"f {indices[3 * i]} {indices[3 * i + 1]} {indices[3 * i + 2]} \n");
        //
        // File.WriteAllText("model.obj", stringBuilder.ToString());


        return new Model(positions, indices.ToArray(), BaseResources.GetCameraShader());

        double ToRads(double input) => input * Double.Pi / 180f;

        uint ToIndex(int k, int i, int j) => (uint)(k * ((int)rows + 1) * ((int)columns + 1)
                                                    + i * ((int)columns + 1)
                                                    + j);
    }

    public static Model LessProceduralModel(uint rows, uint columns)
    {
        var tempVertices = new Vector3[rows + 1, columns + 1];
        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= columns; j++)
            {
                var radius = (float)i / (rows);
                var cos = PosCos(0.5f);
                var sin = PosSin(0.5f);

                //lower quarter
                {
                    tempVertices[i, j] = new Vector3(radius + cos * radius, -radius + sin * radius, radius);
                }


                float PosCos(float offset) => (float)Math.Cos(Math.PI * (offset + 0.5 * ((float)j / columns)));
                float PosSin(float offset) => (float)Math.Sin(Math.PI * (offset + 0.5 * ((float)j / columns)));
            }
        }

        // flatten the array

        var vertices = new Vector3[(rows + 1) * (columns + 1)];

        for (int i = 0; i <= rows; i++)
        for (int j = 0; j <= columns; j++)
            vertices[ToIndex(i, j)] = tempVertices[i, j];

        var indices = new List<uint>();

        for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        {
            indices.Add(ToIndex(i, j));
            indices.Add(ToIndex(i, j + 1));
            indices.Add(ToIndex(i + 1, j));
            indices.Add(ToIndex(i, j + 1));
            indices.Add(ToIndex(i + 1, j));
            indices.Add(ToIndex(i + 1, j + 1));
        }


        double ToRads(double input) => input * Double.Pi / 180f;

        uint ToIndex(int i, int j) => (uint)(i * ((int)columns + 1) + j);

        var positions = new float[vertices.Length * 3];

        foreach (var (vertex, index) in vertices.WithIndex())
        {
            positions[3 * index] = vertex.X / 2;
            positions[3 * index + 1] = vertex.Y / 2;
            positions[3 * index + 2] = vertex.Z / 2;
        }

        return new Model(positions, indices.ToArray(), BaseResources.GetGradientShader());
    }

    public static Model Spline(uint rows, uint cols)
    {
        var spline = new Vector2[] { new(-1, 3), new(2, 1), new(-3, 1), new(2, -1) };

        var pointsV2 = new List<Vector2>();

        for (int i = 0; i <= cols; i++)
        {
            var t = (float)i / cols;
            var a = Lerp(spline[0], spline[1], t);
            var b = Lerp(spline[1], spline[2], t);
            var c = Lerp(spline[2], spline[3], t);

            var d = Lerp(a, b, t);
            var e = Lerp(b, c, t);

            pointsV2.Add(Lerp(d, e, t));
        }

        var pointsV3 = new List<Vector3>();

        for (int j = 0; j <= rows; j++)
            foreach (var pos in pointsV2)
                pointsV3.Add(new Vector3(pos.X, pos.Y, 3.5f * j / cols));

        var indices = new List<uint>();
        
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
        {
            indices.Add(ToIndex(i, j));
            indices.Add(ToIndex(i, j + 1));
            indices.Add(ToIndex(i + 1, j));
            indices.Add(ToIndex(i, j + 1));
            indices.Add(ToIndex(i + 1, j));
            indices.Add(ToIndex(i + 1, j + 1));
        }


        var flattenedPoints = new List<float>();

        foreach (var pos in pointsV3)
        {
            flattenedPoints.Add(pos.X);
            flattenedPoints.Add(pos.Y);
            flattenedPoints.Add(pos.Z);
        }

        return new Model(
            flattenedPoints.ToArray(),
            indices.ToArray(),
            BaseResources.GetCameraShader()
        );

        Vector2 Lerp(Vector2 a, Vector2 b, float t) => a.Scale(t) + b.Scale(1 - t);
        
        uint ToIndex(int i, int j) => (uint)(i * ((int)cols + 1) + j);
    }
}