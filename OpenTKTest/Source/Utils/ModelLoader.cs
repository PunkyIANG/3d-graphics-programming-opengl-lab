using ObjLoader.Loader.Loaders;
using OpenTKTest.Utils;

namespace OpenTKTest;

public static class ModelLoader
{
    public static Model LoadBasicModel(string modelPath)
    {
        var objLoaderFactory = new ObjLoaderFactory();
        var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
        
        var fileStream = new FileStream(modelPath, FileMode.Open);
        var result = objLoader.Load(fileStream);
        
        var model = new Model(result.GetVertices(), result.GetIndices(), BaseResources.GetBasicShader());
        return model;
    }

    private static float[] GetVertices(this LoadResult loadResult)
    {
        var positions = new float[loadResult.Vertices.Count * 3];

        foreach (var (vertex, index) in loadResult.Vertices.WithIndex())
        {
            positions[3 * index]     = vertex.X;
            positions[3 * index + 1] = vertex.Y;
            positions[3 * index + 2] = vertex.Z;
        }

        return positions;
    }

    private static uint[] GetIndices(this LoadResult loadResult)
    {
        var faces = loadResult.Groups
            .SelectMany(group => group.Faces)
            .ToArray();

        var indices = new uint[faces.Length * 3];
        
        foreach (var (face, index) in faces.WithIndex())
        {
            indices[3 * index + 1] = (uint) face[1].VertexIndex - 1;
            indices[3 * index + 2] = (uint) face[2].VertexIndex - 1;
            indices[3 * index]     = (uint) face[0].VertexIndex - 1;
        }

        return indices;
    }
}