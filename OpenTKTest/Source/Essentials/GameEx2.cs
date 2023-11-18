using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Source.Utils;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;

namespace OpenTKTest;

public class GameEx2 : GameWindow
{
    private const double frameTime = 0.05;

    private Model[] _models;

    private static DebugProc DebugMessageDelegate = OnDebugMessage;
    private readonly Camera _camera;
    private bool _firstMove = true;
    private Vector2 _lastPos;
    private double _time;


    public GameEx2(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title,
            Flags = ContextFlags.Debug
        })
    {
        CursorState = CursorState.Grabbed;
        _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
    }

    ///basically start
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);

        GL.Enable(EnableCap.DebugOutput);


// Optionally
        GL.Enable(EnableCap.DebugOutputSynchronous);


        // GL.ClearColor(0.2f, 0.6f, 1f, 1.0f);
        // GL.ClearColor(1f, 1f, 1f, 1f);
        GL.ClearColor(0.2f, 0.5f, 0.6f, 1.0f);

        _models = new[] { Prefabs.Spline(50, 50) };
        // _models = new[] { Prefabs.GetGradientQuad() };
        // _models = new[] { Prefabs.GetTripleArrow()};
        // _models = new[] { Prefabs.GetArrow(), Prefabs.GetArrow(), Prefabs.GetArrow() };
        // _models = new[] { ModelLoader.LoadQuad() };
        // _models = new[] { Prefabs.GetTexturedQuad() };
        // _models = new[] { Prefabs.GetArrow(), Prefabs.GetQuad() };
        // _models[0].Color = Vector4.One;

        // _models[1].Rotation += new Vector3(0, 0, 0);
        // _models[2].Rotation += new Vector3(0, 0, 0);

        // foreach (var model in _models)
        // {
        //     model.Color = new Vector4(0, 0, 0, 1);
        //     model.Rotation += new Vector3(35f, -35f, -21.9f);
        // }
    }

    /// good for handling input before stuff gets rendered
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

        // foreach (var model in _models)
        // {
        //     model.Rotation += (float)e.Time * Vector3.One;
        //     // model.Rotation += (float)e.Time * Vector3.UnitZ;
        // }

        if (!IsFocused) // Check to see if the window is focused
        {
            return;
        }

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (input.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        if (input.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }

        if (input.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        }

        if (input.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        }

        if (input.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        }

        if (input.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        // Get the mouse state
        var mouse = MouseState;

        if (_firstMove) // This bool variable is initially set to true.
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            // Calculate the offset of the mouse position
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
        }
    }

    /// actual rendering code here
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Disable(EnableCap.CullFace);

        //vsync n schiit
        if (e.Time < frameTime)
            Thread.Sleep((int)((frameTime - e.Time) * 1000));
        // if (!IsFocused) return;
        
        _time += 4.0 * e.Time;



        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var model in _models)
        {
            model.Shader.Use();
            
            var modelMatrix4 = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            model.Shader.SetMatrix4("model", modelMatrix4);
            model.Shader.SetMatrix4("view", _camera.GetViewMatrix());
            model.Shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            model.OnRenderFrame(e);
        }

        // ALWAYS run last
        SwapBuffers();
        // local space
        // to world space
        // to view space
        // to clip space
        // to screen space
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private static void OnDebugMessage(
        DebugSource source, // Source of the debugging message.
        DebugType type, // Type of the debugging message.
        int id, // ID associated with the message.
        DebugSeverity severity, // Severity of the message.
        int length, // Length of the string in pMessage.
        IntPtr pMessage, // Pointer to message string.
        IntPtr pUserParam) // The pointer you gave to OpenGL, explained later.
    {
        // In order to access the string pointed to by pMessage, you can use Marshal
        // class to copy its contents to a C# string without unsafe code. You can
        // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
        string message = Marshal.PtrToStringAnsi(pMessage, length);

        // The rest of the function is up to you to implement, however a debug output
        // is always useful.
        Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);

        // Potentially, you may want to throw from the function for certain severity
        // messages.
        if (type == DebugType.DebugTypeError)
        {
            throw new Exception(message);
        }
    }
}