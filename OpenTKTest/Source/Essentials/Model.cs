﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTKTest.Source.Utils;

namespace OpenTKTest;

public class Model
{
    private readonly uint[] _indices;
    private readonly int _vertexArrayObject;
    public readonly Shader Shader;
    public Vector4 Color { get; set; } = new Vector4(0,0,0,1);
    
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    
    public Vector3 FirstPos { get; set; }
    public Vector3 SecondPos { get; set; }

    public Vector4 FirstColor { get; set; }
    public Vector4 SecondColor { get; set; }

    // stretch goals
    // transformation matrix
    // deduced from position, rotation, etc
    
    public Model(float[] vertices, uint[] indices, Shader shader)
    {
        _indices = indices;
        Shader = shader;

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        var vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        var elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }

    public void OnRenderFrame(FrameEventArgs e)
    { 
        var transform = Matrix4.Identity;
        
        transform *= Matrix4.CreateScale(Scale);
        transform *= Matrix4.CreateTranslation(Position);
        //do all rotations in one go

        var radRotation = Rotation.DegreesToRads();
        
        transform *= Matrix4.CreateRotationX(radRotation.X);
        transform *= Matrix4.CreateRotationY(radRotation.Y);
        transform *= Matrix4.CreateRotationZ(radRotation.Z);

        GL.BindVertexArray(_vertexArrayObject);
        // Shader.Use();
        
        Shader.SetMatrix4("transform", transform);
        Shader.SetVector4("color", Color);
        
        Shader.SetVector4("firstColor", FirstColor);
        Shader.SetVector4("secondColor", SecondColor);
        
        Shader.SetVector3("firstPos", FirstPos);
        Shader.SetVector3("secondPos", SecondPos);

        
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}