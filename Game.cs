
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
  int _lightVAO;
  int _elementBufferObject;

  Shader _shader;
  Shader _lampShader;

  Vector3 _lightPos = (0.0f, 0.0f, 0.0f);

  Texture _textureDiffuse;
  Texture _textureSpecular;
  Texture _textureEmission;
  float _mixCoefficient;


  InputCallback _input;


  float _lastFrame = 0.0f;
  float _deltaTime = 0.0f;

  Camera _camera;

  Stopwatch _timer = new Stopwatch();

  List<Vector3> _cubePositions = new List<Vector3>{
    new Vector3( 0.0f,  10.0f,  0.0f),
    new Vector3( 2.0f,  5.0f, -15.0f),
    new Vector3(-1.5f, -2.2f, -2.5f),
    new Vector3(-3.8f, -2.0f, -12.3f),
    new Vector3( 2.4f, -0.4f, -3.5f),
    new Vector3(-1.7f,  3.0f, -7.5f),
    new Vector3( 1.3f, -2.0f, -2.5f),
    new Vector3( 1.5f,  2.0f, -2.5f),
    new Vector3( 1.5f,  0.2f, -1.5f),
    new Vector3(-1.3f,  1.0f, -1.5f),
    new Vector3(2.0f,  1.0f, 10.5f)
    // new Vector3(3.0f, 0.0f, 0.0f),
    // new Vector3(0.0f, 0.0f, 3.0f),
    // new Vector3(-3.0f, 0.0f, 0.0f),
    // new Vector3(0.0f, 0.0f, -3.0f),


    };


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


    float[] vertices = {
        // vertexes             // normals              //texture coords
        -0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,    0.0f,  0.0f, -1.0f,     0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,    0.0f,  0.0f,  1.0f,     0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,     1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,     1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,     0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,     0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,     0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,     1.0f, 0.0f,

         0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,     1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,    1.0f,  0.0f,  0.0f,     1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,     0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,     0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,    1.0f,  0.0f,  0.0f,     0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,     1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,     0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,     1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,     1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,     1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,     0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,    0.0f, -1.0f,  0.0f,     0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,     0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,     1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,     1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,     1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,     0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,     0.0f, 1.0f
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

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
    GL.EnableVertexAttribArray(0);

    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
    GL.EnableVertexAttribArray(1);

    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
    GL.EnableVertexAttribArray(2);

    GL.BindVertexArray(0);

    // light VAO
    _lightVAO = GL.GenVertexArray();
    GL.BindVertexArray(_lightVAO);

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
    GL.EnableVertexAttribArray(0);

    GL.BindVertexArray(0);



    //EBO
    _elementBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

    _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");

    _lampShader = new Shader("shaders/shader.vert", "shaders/lamp_shader.frag");

    _textureDiffuse = new Texture("/home/maksim/Pictures/container2.png");
    _textureSpecular = new Texture("/home/maksim/Pictures/container2_specular.png");
    _textureEmission = new Texture("/home/maksim/Pictures/matrix.jpg");

    _input = new InputCallback(KeyboardState, MouseState);
    _camera = new Camera(new Vector3(-2.0f, 1.0f, 0.0f), new Vector3(2.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
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
    System.Threading.Thread.Sleep(20);
    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    float current_frame = _timer.ElapsedMilliseconds;
    _deltaTime = current_frame - _lastFrame;
    _lastFrame = current_frame;
    InputData input_data = _input.GetInputData();

    // float x_pos = (float)Math.Sin(MathHelper.DegreesToRadians(15*_timer.Elapsed.TotalSeconds)) * 5;
    // float y_pos = (float)Math.Cos(MathHelper.DegreesToRadians(15*_timer.Elapsed.TotalSeconds)) * 5;
    // _lightPos.X = x_pos;
    // _lightPos.Y = y_pos;
    //camera
    _camera.Update(input_data, _deltaTime);
    Matrix4 view = _camera.GetLookAt();
    Matrix4 projection = _camera.GetProjection();

    Vector3 camera_pos = _camera.Pos;

    //cube

    _shader.Use();
    _textureDiffuse.Use(TextureUnit.Texture0);
    _shader.SetInt("material.diffuse", 0);
    _textureSpecular.Use(TextureUnit.Texture1);
    _shader.SetInt("material.specular", 1);
    _textureEmission.Use(TextureUnit.Texture2);
    _shader.SetInt("material.emission", 2);


    _shader.SetMat4("projection", projection);
    _shader.SetMat4("view", view);

    _shader.SetVec3("viewPos", camera_pos);

    _shader.SetFloat("material.shininess", 100.0f);

    float x_pos = (float)Math.Sin(MathHelper.DegreesToRadians(15 * _timer.Elapsed.TotalSeconds));
    float y_pos = (float)Math.Cos(MathHelper.DegreesToRadians(15 * _timer.Elapsed.TotalSeconds));

    _shader.SetVec3("pLight.ambient", new Vector3(0.1f, 0.1f, 0.1f));
    _shader.SetVec3("pLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f)); // darken the light a bit to fit the scene
    _shader.SetVec3("pLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
    _shader.SetVec3("pLight.position", _lightPos);
    _shader.SetFloat("pLight.constant",  1.0f);
    _shader.SetFloat("pLight.linear",    0.14f);
    _shader.SetFloat("pLight.quadratic", 0.07f);


    _shader.SetVec3("dirLight.direction", new Vector3(1.0f, 0.0f, 0.5f));
    _shader.SetVec3("dirLight.diffuse", new Vector3(1.0f, 0.0f, 0.0f));

    _shader.SetVec3("spotlight.position", _camera.Pos);
    _shader.SetVec3("spotlight.direction", _camera.Direction);
    _shader.SetFloat("spotlight.phi", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
    _shader.SetVec3("spotlight.ambient", new Vector3(0.1f, 0.1f, 0.1f));
    _shader.SetVec3("spotlight.diffuse", new Vector3(1.0f, 1.0f, 1.0f)); // darken the light a bit to fit the scene
    _shader.SetVec3("spotlight.specular", new Vector3(1.0f, 1.0f, 1.0f));    

    GL.BindVertexArray(_vertexArrayObject);

    for (int i = 0; i < _cubePositions.Count; i++)
    {
      Matrix4 model = Matrix4.Identity;
      model *= Matrix4.CreateTranslation(_cubePositions[i]);
      // We then calculate the angle and rotate the model around an axis
      float angle = 20.0f * i;
      model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
      // Remember to set the model at last so it can be used by opentk
      _shader.SetMat4("model", model);

      // At last we draw all our cubes
      GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }




    //lamp
    Matrix4 lamp_model = Matrix4.CreateScale(0.2f);
    lamp_model *= Matrix4.CreateTranslation(_lightPos);

    _lampShader.Use();
    //    _lampShader.SetFloat("mix_coefficient", _mixCoefficient);
    _lampShader.SetMat4("projection", projection);
    _lampShader.SetMat4("view", view);
    _lampShader.SetMat4("model", lamp_model);

    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


    Context.SwapBuffers();

    base.OnRenderFrame(e);

  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);

    GL.Viewport(0, (e.Height - e.Width)/2, e.Width, e.Width);
  }



}