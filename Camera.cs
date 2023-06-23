using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Camera
{
  Vector3 _pos = new(0.0f, 0.0f, 0.0f);
  Vector3 _direction = new(0.0f, -1.0f, -1.0f);
  Vector3 _up = new(0.0f, 1.0f, 0.0f);
  float _yaw = -90.0f;
  float _pitch = 0.0f;
  float _fov = 45.0f;

  bool[] _keys = new bool[1024];

  Camera(Vector3 pos, Vector3 direction, Vector3 up, float fov = 45.0f, float yaw = -90.0f, float pitch = 0.0f)
  {
    _pos = pos;
    _direction = direction;
    _up = up;
    _yaw = yaw;
    _pitch = pitch;
    _fov = fov;
  }

  public Matrix4 GetLookAt()
  {
    return Matrix4.LookAt(_pos, _pos + _direction, _up);
  }

  public void Update(KeyboardState input)
  {
    KeyCallback(input);
    DoMovement();
  }


  protected void KeyCallback(KeyboardState input)
  {

    List<Keys> tracked_keys = new List<Keys> { Keys.W, Keys.S, Keys.D, Keys.A };

    foreach (Keys key in tracked_keys)
    {
      if (input.IsKeyPressed(key))
      {
        _keys[(int)key] = true;
      }
      else if (input.IsKeyReleased(key))
      {
        _keys[(int)key] = false;
      }
    }
  }

  private void DoMovement()
  {
    float camera_speed = 0.02f;

    if (_keys[(int)Keys.W])
    {
      _pos += _direction * camera_speed * _deltaTime;
    }
    if (_keys[(int)Keys.S])
    {
      _pos -= _direction * camera_speed * _deltaTime;
    }
    if (_keys[(int)Keys.D])
    {
      _pos += Vector3.Cross(_direction, _up) * camera_speed * _deltaTime;
    }
    if (_keys[(int)Keys.A])
    {
      _pos += Vector3.Cross(_up, _direction) * camera_speed * _deltaTime;
    }
  }

  private void DoRotating()
  {
    float sensitivity = 0.04f;
    _pitch -= _deltaMousePos.Y * sensitivity;
    _yaw += _deltaMousePos.X * sensitivity;

    if (_pitch > 89.0f)
      _pitch = 89.0f;
    if (_pitch < -89.0f)
      _pitch = -89.0f;

    float pitch = MathHelper.DegreesToRadians(_pitch);
    float yaw = MathHelper.DegreesToRadians(_yaw);

    Vector3 direction = new Vector3();
    direction.X = (float)(Math.Cos(pitch) * Math.Cos(yaw));
    direction.Y = (float)(Math.Sin(pitch));
    direction.Z = (float)(Math.Cos(pitch) * Math.Sin(yaw));

    _direction = Vector3.Normalize(direction);
  }

  private void MouseCallback()
  {
    _deltaMousePos.X = MousePosition.X - _lastMousePos.X;
    _deltaMousePos.Y = MousePosition.Y - _lastMousePos.Y;
    _lastMousePos = MousePosition;

  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);


    _fov -= e.OffsetY;

    if (_fov >= 45.0f)
    {
      _fov = 45.0f;
    }
    else if (_fov <= 1.0f)
    {
      _fov = 1.0f;
    }

  }
}