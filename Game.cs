
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System.Diagnostics;


public class Game : GameWindow
{
  int VertexBufferObject;
  int VertexArrayObject;
  int VertexBufferObject2;
  int VertexArrayObject2;
  int ElementBufferObject;
  Shader shader;
  Shader shader_2;
//  Stopwatch _timer = new Stopwatch();
  Texture texture;

  public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

  protected override void OnUpdateFrame(FrameEventArgs e)
  {
    base.OnUpdateFrame(e);
    KeyboardState input = KeyboardState;

    if (input.IsKeyDown(Keys.Escape))
    {
      Close();
    }
  }
  protected override void OnLoad()
  {
    base.OnLoad();

    GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

    float[] vertices =
    {
    //Position            Texture coordinates   colors
     0.5f,  0.5f, 0.0f,   1.0f, 1.0f,           1.0f, 0.0f, 0.0f,// top right
     0.5f, -0.5f, 0.0f,   1.0f, 0.0f,           0.0f, 1.0f, 0.0f,// bottom right
    -0.5f, -0.5f, 0.0f,   0.0f, 0.0f,           0.0f, 0.0f, 1.0f,// bottom left
    -0.5f,  0.5f, 0.0f,   0.0f, 1.0f,           1.0f, 1.0f, 0.0f,// top left
    };



    uint[] indices = {
    0, 1, 3, // First Triangle
    1, 2, 3  // Second Triangle
    };

    //VBO1
    VertexBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

    //VAO1
    VertexArrayObject = GL.GenVertexArray();
    GL.BindVertexArray(VertexArrayObject);

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
    GL.EnableVertexAttribArray(0);

    GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
    GL.EnableVertexAttribArray(1);

    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
    GL.EnableVertexAttribArray(2);

    //VBO2
    // VertexBufferObject2 = GL.GenBuffer();
    // GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject2);
    // GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * sizeof(float), vertices2, BufferUsageHint.StaticDraw);

    //VAO2
    // VertexArrayObject2 = GL.GenVertexArray();
    // GL.BindVertexArray(VertexArrayObject2);
    // GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
    // GL.EnableVertexAttribArray(0);


    //EBO
    ElementBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

    shader = new Shader("shader.vert", "shader.frag");
    //    shader_2 = new Shader("shader.vert", "shader_green.frag");


    texture = new Texture("/home/maksim/Pictures/container.jpg");
  }

  protected override void OnUnload()
  {
    base.OnUnload();
    shader.Dispose();
  }
  protected override void OnRenderFrame(FrameEventArgs e)
  {
    base.OnRenderFrame(e);

    GL.Clear(ClearBufferMask.ColorBufferBit);

    //Code goes here.
    shader.Use();

//1
    texture.Use();
    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

    //    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    // GL.BindVertexArray(VertexArrayObject);
    // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

    // shader_2.Use();
    // GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 3);
    // GL.BindVertexArray(VertexArrayObject2);
    // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

    //    GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    SwapBuffers();
  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);

    GL.Viewport(0, 0, e.Width, e.Height);
  }

}