using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKTest;

public class ModernGame : GameWindow
{
    private Model[] _models =
    {
        // new Model()
    };
    
    public ModernGame(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { Size = (width, height), Title = title }) { }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        
        GL.ClearColor(0f, 0f, 0f, 1.0f);

        //basically start
    }
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        //good for handling input before stuff gets rendered
        
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();

        //actual rendering code here
        
        // local space
        // to world space
        // to view space
        // to clip space
        // to screen space
    }
}