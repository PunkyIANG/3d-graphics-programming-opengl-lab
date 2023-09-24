using ObjLoader.Loader.Loaders;

namespace OpenTKTest.Source.Utils;

public class CustomMaterialStreamProvider : IMaterialStreamProvider
{
    private readonly string materialPath;
    public CustomMaterialStreamProvider(string materialPath)
    {
        this.materialPath = materialPath;
    }

    public Stream Open(string materialFilePath)
    {
        return (Stream) File.Open(materialPath, FileMode.Open, FileAccess.Read);    
    }
}