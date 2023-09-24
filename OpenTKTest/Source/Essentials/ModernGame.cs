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

public class ModernGame : GameWindow
{
    private const double frameTime = 0.05;

    private Model[] _models;
    
    private static DebugProc DebugMessageDelegate = OnDebugMessage;
    
    public ModernGame(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title,
            Flags = ContextFlags.Debug
        }) { }
    
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
        

        _models = new[] { Prefabs.GetArrow(), Prefabs.GetArrow(), Prefabs.GetArrow() };
        // _models = new[] { ModelLoader.LoadQuad() };
        // _models = new[] { Prefabs.GetTexturedQuad() };
        // _models = new[] { Prefabs.GetArrow(), Prefabs.GetQuad() };
        // _models[0].Color = Vector4.One;

        _models[1].Rotation += new Vector3(0, 90, 0);
        _models[2].Rotation += new Vector3(0, 0, 90);

        foreach (var model in _models)
        {
            model.Color = new Vector4(0, 0, 0, 1);
            model.Rotation += new Vector3(35f, 35f, 35f);
        }
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

        
        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var model in _models) 
            model.OnRenderFrame(e);

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