using System.Text;
using OpenTK.Mathematics;
using OpenTKTest.Utils;

namespace OpenTKTest.Source.Utils;

public static class Prefabs
{
    public static Model GetQuad() => ModelLoader.LoadBasicModel("Models/Quad/quad.obj");

    public static Model GetTripleArrow() =>
        ModelLoader.LoadTexturedModel("Models/ThreeArrows/threearrows.obj", "Models/ThreeArrows/threearrows.mtl");

    public static Model GetArrow() => ModelLoader.LoadBasicModel("Models/Arrow/arrow.obj");

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
            tempVertices[i, j, 0] = new Vector3((radius + PosCos(0.5f) * radius) * 2, (-radius + PosSin(0.5f) * radius) * 2, radius * 2);
            
            //circle stuff
            tempVertices[i, j, 1] = new Vector3((-radius + PosCos(0.5f)) * 1.5f * radius, radius + PosSin(0.5f) * radius, radius * 2);
            tempVertices[i, j, 2] = new Vector3((-radius + PosCos(0f)) * 1.5f * radius, radius + PosSin(0f) * radius, radius * 2);
            tempVertices[i, j, 3] = new Vector3((-radius + PosCos(1f)) * 1.5f * radius, radius + PosSin(1f) * radius, radius * 2);
            tempVertices[i, j, 4] = new Vector3((-radius + PosCos(1.5f)) * 1.5f * radius, radius + PosSin(1.5f) * radius, radius * 2);
            
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

            uint offset = (uint) positions.Length / 2;
            
            indices.Add(offset + ToIndex(k, i, j));
            indices.Add(offset + ToIndex(k, i, j + 1));
            indices.Add(offset + ToIndex(k, i + 1, j));
            indices.Add(offset + ToIndex(k, i, j + 1));
            indices.Add(offset + ToIndex(k, i + 1, j));
            indices.Add(offset + ToIndex(k, i + 1, j + 1));
        }

        var stringBuilder = new StringBuilder();

        foreach (var vertex in vertices) 
            stringBuilder.Append($"v {vertex.X} {vertex.Y} {vertex.Z} \n");

        for (int i = 0; i < indices.Count / 3; i++)
            stringBuilder.Append($"f {indices[3 * i]} {indices[3 * i + 1]} {indices[3 * i + 2]} \n");

        File.WriteAllText("model.obj", stringBuilder.ToString());


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

        uint ToIndex(int i, int j) =>(uint)(i * ((int)columns + 1) + j);

        var positions = new float[vertices.Length * 3];

        foreach (var (vertex, index) in vertices.WithIndex())
        {
            positions[3 * index] = vertex.X / 2;
            positions[3 * index + 1] = vertex.Y / 2;
            positions[3 * index + 2] = vertex.Z / 2;
        }

        return new Model(positions, indices.ToArray(), BaseResources.GetGradientShader());
    }

}