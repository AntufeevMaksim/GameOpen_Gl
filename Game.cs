
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


public class Game : GameWindow
{
  int VertexBufferObject;
  int VertexArrayObject;
  Shader shader;
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

    //Code goes here
    float[] vertices = {
    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
     0.5f, -0.5f, 0.0f, //Bottom-right vertex
     0.0f,  0.5f, 0.0f  //Top vertex
    };

    VertexBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

    shader = new Shader("shader.vert", "shader.frag");

    VertexArrayObject = GL.GenVertexArray();
    GL.BindVertexArray(VertexArrayObject);

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
    GL.EnableVertexAttribArray(0);

    shader.Use();


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
    GL.BindVertexArray(VertexArrayObject);
    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


    SwapBuffers();
  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);

    GL.Viewport(0, 0, e.Width, e.Height);
  }

}