
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using Input;

using System.Diagnostics;

public class Game : GameWindow
{
  int _vertexBufferObject;
  int _vertexArrayObject;

  int _elementBufferObject;

  Shader _shader;
  Shader _shader2;

  Texture _texture0;
  Texture _texture1;

  float _mixCoefficient;
  float _rotationAngle;
  // KeyboardState _input;
  // Vector2 _lastMousePos;
  // Vector2 _deltaMousePos;

  InputCallback _input;

  // float _yaw = -90.0f;
  // float _pitch = 0.0f;
  // float _fov = 45.0f;
//  bool[] _keys = new bool[1024];

  // Vector3 _cameraPos = new(0.0f, 0.0f, 0.0f);
  // Vector3 _cameraFront = new(0.0f, -1.0f, -1.0f);
  // Vector3 _cameraUp = new(0.0f, 1.0f, 0.0f);

  float _lastFrame = 0.0f;
  float _deltaTime = 0.0f;

  Camera _camera;

  Stopwatch _timer = new Stopwatch();
  public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

  protected override void OnUpdateFrame(FrameEventArgs e)
  {
    base.OnUpdateFrame(e);
    KeyboardState input = KeyboardState;

    if (input.IsKeyDown(Keys.Escape))
    {
      Close();
    }

    if (input.IsKeyPressed(Keys.Up) & _mixCoefficient < 1)
    {
      _mixCoefficient += 0.1f;
    }
    else if (input.IsKeyPressed(Keys.Down) & _mixCoefficient > 0)
    {
      _mixCoefficient -= 0.1f;
    }
  }
  protected override void OnLoad()
  {
    base.OnLoad();

    GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

    // float[] vertices =
    // {
    // //Position            Texture coordinates   colors
    //  0.5f,  0.5f, 0.0f,   2.0f, 2.0f,           1.0f, 0.0f, 0.0f,// top right
    //  0.5f, -0.5f, 0.0f,   2.0f, 0.0f,           0.0f, 1.0f, 0.0f,// bottom right
    // -0.5f, -0.5f, 0.0f,   0.0f, 0.0f,           0.0f, 0.0f, 1.0f,// bottom left
    // -0.5f,  0.5f, 0.0f,   0.0f, 2.0f,           1.0f, 1.0f, 0.0f,// top left
    // };

    float[] vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    };



    uint[] indices = {
    0, 1, 3, // First Triangle
    1, 2, 3  // Second Triangle
    };

    //VBO1
    _vertexBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

    //VAO1
    _vertexArrayObject = GL.GenVertexArray();
    GL.BindVertexArray(_vertexArrayObject);

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
    GL.EnableVertexAttribArray(0);

    GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
    GL.EnableVertexAttribArray(1);

    // GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
    // GL.EnableVertexAttribArray(2);

    //EBO
    _elementBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

    _shader = new Shader("shader.vert", "shader.frag");
    //    shader_2 = new Shader("shader.vert", "shader_green.frag");


    _texture0 = new Texture("/home/maksim/Pictures/container.jpg");
    _texture1 = new Texture("/home/maksim/Pictures/awesomeface.png");

//    _input = KeyboardState;
    _input = new InputCallback(KeyboardState, MouseState);
    _camera = new Camera(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, -1.0f), new Vector3(0.0f, 1.0f, 0.0f));
    CursorState = CursorState.Grabbed;
    _timer.Start();

    GL.Enable(EnableCap.DepthTest);
  }

  protected override void OnUnload()
  {
    base.OnUnload();
    _shader.Dispose();
  }
  protected override void OnRenderFrame(FrameEventArgs e)
  {

    float current_frame = _timer.ElapsedMilliseconds;
    _deltaTime = current_frame - _lastFrame;
    _lastFrame = current_frame;

    InputData input_data = _input.GetInputData();

    _camera.Update(input_data, _deltaTime);
    Matrix4 view = _camera.GetLookAt();
    Matrix4 projection = _camera.GetProjection();
    // KeyCallback();
    // DoMovement(input_data);
    // MouseCallback();
    // DoRotating(input_data);

    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


    // Matrix4 view = Matrix4.LookAt(_cameraPos,
    //      _cameraPos + _cameraFront,
    //      _cameraUp);

    // Matrix4 view = LookAt(_cameraPos,
    //      _cameraPos + _cameraFront,
    //      _cameraUp);
    Matrix4 rotation = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 1), MathHelper.DegreesToRadians(0 * (float)_timer.Elapsed.TotalSeconds));
    Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0 * (float)_timer.Elapsed.TotalSeconds));
//    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), 800 / 600, 0.1f, 100.0f);
    Matrix4 trans = rotation * view * projection;

    _shader.Use();
    _texture0.Use(TextureUnit.Texture0);
    _shader.SetInt("texture0", 0);
    _texture1.Use(TextureUnit.Texture1);
    _shader.SetInt("texture1", 1);

    _shader.SetFloat("mix_coefficient", _mixCoefficient);
    _shader.SetMat4("transform", trans);


    GL.BindVertexArray(_vertexArrayObject);

    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

    trans = Matrix4.CreateTranslation(0.5f, 0.0f, 3.0f);
    _shader.SetMat4("transform", trans * view * projection);

    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

    // GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

    Context.SwapBuffers();

    base.OnRenderFrame(e);

  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);

    GL.Viewport(0, 0, e.Width, e.Width);
  }



}