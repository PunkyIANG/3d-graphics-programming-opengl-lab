using ObjLoader.Loader.Loaders;
using OpenTKTest.Source.Utils;
using OpenTKTest.Utils;

namespace OpenTKTest;

public static class ModelLoader
{
    private static float[] _quadVertices =
    {
        0.5f, 0.5f, 0.0f, // top right
        0.5f, -0.5f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f // top left
    };

    private static uint[] _quadIndices =
    {
        // note that we start from 0!
        0, 1, 3, // first triangle
        1, 2, 3 // second triangle
    };

    public static Model LoadBasicModel(string objPath)
    {
        var objLoaderFactory = new ObjLoaderFactory();
        var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
        // var objLoader = objLoaderFactory.Create(new MaterialStreamProvider());
        // var objLoader = objLoaderFactory.Create(new CustomMaterialStreamProvider());

        using Stream fileStream = new FileStream(objPath, FileMode.Open);
        // var fileStream = new FileStream(objPath, FileMode.Open);
        var result = objLoader.Load(fileStream);

        var model = new Model(result.GetVertices(), result.GetIndices(), BaseResources.GetFunkyShader());
        return model;
    }

    public static Model LoadTexturedModel(string objPath, string materialPath)
    {
        var objLoaderFactory = new ObjLoaderFactory();
        var objLoader = objLoaderFactory.Create(new CustomMaterialStreamProvider(materialPath));

        var fileStream = new FileStream(objPath, FileMode.Open);
        var result = objLoader.Load(fileStream);

        var model = new Model(result.GetVertices(), result.GetIndices(), BaseResources.GetFunkyShader());
        return model;
    }


    public static Model LoadQuad()
    {
        var model = new Model(_quadVertices, _quadIndices, BaseResources.GetBasicShader());
        return model;
    }

    private static float[] GetVertices(this LoadResult loadResult)
    {
        var positions = new float[loadResult.Vertices.Count * 3];

        foreach (var (vertex, index) in loadResult.Vertices.WithIndex())
        {
            positions[3 * index] = vertex.X;
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
            indices[3 * index + 1] = (uint)face[1].VertexIndex - 1;
            indices[3 * index + 2] = (uint)face[2].VertexIndex - 1;
            indices[3 * index] = (uint)face[0].VertexIndex - 1;
        }

        return indices;
    }
}