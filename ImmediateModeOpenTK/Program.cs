using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

using (var game = new GameWindow())
{
    game.Load += (sender, e) =>
    {
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        GL.Rotate(35f, Vector3.UnitY);
        GL.Rotate(-35f, Vector3.UnitX);

    };

    game.RenderFrame += (sender, e) =>
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Begin(PrimitiveType.TriangleStrip);
        GL.Vertex3(-0.5f, -0.5f, -0.5);
        GL.Vertex3(0.5f, -0.5f, -0.5);
        GL.Vertex3(-0.5f, 0.5f, -0.5);
        GL.Vertex3(0.5f, 0.5f, -0.5);
        GL.End();
        
        GL.Begin(PrimitiveType.TriangleStrip);
        GL.Vertex3(-0.5f, -0.5f, 0.5);
        GL.Vertex3(0.5f, -0.5f, 0.5);
        GL.Vertex3(-0.5f, 0.5f, 0.5);
        GL.Vertex3(0.5f, 0.5f, 0.5);
        GL.End();
        

        game.SwapBuffers();
    };

    game.Run(60.0);
}