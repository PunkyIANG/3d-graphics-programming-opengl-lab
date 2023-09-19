using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKTest;

public class Game : GameWindow
{
    private const double frameTime = 0.05;
    private double gameTimer = 0;
    
    float[] vertices = {
        0.5f,  0.5f, 0.0f,  // top right
        0.5f, -0.5f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  // bottom left
        -0.5f,  0.5f, 0.0f   // top left
    };
    
    uint[] indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    private int VertexBufferObject;
    private int VertexArrayObject;
    private int ElementBufferObject;
    private Shader _shader;
    

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { Size = (width, height), Title = title }) { }


    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.5f, 0.6f, 1.0f);

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);
        
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        _shader = new Shader("Shaders/basic-shader.vert", "Shaders/basic-shader.frag");
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        if (e.Time < frameTime)
            Thread.Sleep((int)((frameTime - e.Time) * 1000));
        
        if (!IsFocused) return;
        
        GL.Clear(ClearBufferMask.ColorBufferBit);

        var transform = Matrix4.Identity;

        gameTimer += e.Time;
        {
            var angleOfRotation = (float)MathHelper.DegreesToRadians(gameTimer * 100);
            transform *= Matrix4.CreateRotationX(angleOfRotation);
            transform *= Matrix4.CreateRotationZ(angleOfRotation);
        }

        GL.BindVertexArray(VertexArrayObject);
        _shader.Use();
        // _shader.SetMatrix4("transform", transform);

        // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        
        // _shader.Dispose();
    }
}