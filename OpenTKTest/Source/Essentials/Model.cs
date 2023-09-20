using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace OpenTKTest;

public class Model
{
    private readonly uint[] _indices;
    private readonly int _vertexArrayObject;
    private readonly Shader _shader;
    
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; }
    
    // stretch goals
    // transformation matrix
    // deduced from position, rotation, etc
    
    public Model(float[] vertices, uint[] indices, Shader shader)
    {
        _indices = indices;
        _shader = shader;
        
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        var VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        var ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }

    public void OnRenderFrame(FrameEventArgs e)
    {
        var transform = Matrix4.Identity;
        
        transform *= Matrix4.CreateScale(Scale);
        transform *= Matrix4.CreateTranslation(Position);
        transform *= Matrix4.CreateRotationX(Rotation.X);
        transform *= Matrix4.CreateRotationY(Rotation.Y);
        transform *= Matrix4.CreateRotationZ(Rotation.Z);

        GL.BindVertexArray(_vertexArrayObject);
        _shader.Use();
        // _shader.SetMatrix4("transform", transform);
        
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}