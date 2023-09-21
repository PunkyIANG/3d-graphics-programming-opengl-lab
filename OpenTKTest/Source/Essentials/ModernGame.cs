using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKTest.Source.Utils;

namespace OpenTKTest;

public class ModernGame : GameWindow
{
    private const double frameTime = 0.05;

    private Model[] _models;
    
    public ModernGame(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { Size = (width, height), Title = title }) { }
    
    ///basically start
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.6f, 1f, 1.0f);

        _models = new[] { Prefabs.GetQuad() };
    }
    
    /// good for handling input before stuff gets rendered
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
    }

    /// actual rendering code here
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        //vsync n schiit
        if (e.Time < frameTime)
            Thread.Sleep((int)((frameTime - e.Time) * 1000));
        if (!IsFocused) return;

        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();

        foreach (var model in _models) 
            model.OnRenderFrame(e);

        // local space
        // to world space
        // to view space
        // to clip space
        // to screen space
    }
}