namespace OpenTKTest;

public static class BaseResources
{
    public static Shader GetBasicShader() => new("Shaders/basic-shader.vert", "Shaders/basic-shader.frag");
    
}