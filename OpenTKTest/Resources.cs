namespace OpenTKTest;

public static class Resources
{
    public static Shader GetBasicShader() => new("Shader/basic-shader.vert", "Shader/basic-shader.frag");
}