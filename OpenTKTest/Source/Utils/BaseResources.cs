namespace OpenTKTest;

public static class BaseResources
{
    public static Shader GetBasicShader() => new("Shaders/basic-shader.vert", "Shaders/basic-shader.frag");
    public static Shader GetFunkyShader() => new("Shaders/spinning-shader.vert", "Shaders/spinning-shader.frag");
    public static Shader GetUsefulShader() => new("Shaders/spinning-shader.vert", "Shaders/basic-shader.frag");
}