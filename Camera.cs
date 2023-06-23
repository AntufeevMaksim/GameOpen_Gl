using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Input;

class Camera
{
  Vector3 _pos = new(0.0f, 0.0f, 0.0f);
  Vector3 _direction = new(0.0f, -1.0f, -1.0f);
  Vector3 _up = new(0.0f, 1.0f, 0.0f);
  float _yaw = -90.0f;
  float _pitch = 0.0f;
  float _fov = 45.0f;

  bool[] _keys = new bool[1024];

  public Camera(Vector3 pos, Vector3 direction, Vector3 up, float fov = 45.0f, float yaw = -90.0f, float pitch = 0.0f)
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

  public Matrix4 GetProjection()
  {
    return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), 800 / 600, 0.1f, 100.0f);
  }
  public void Update(InputData input, float deltaTime)
  {
    DoMovement(input, deltaTime);
    DoRotating(input, deltaTime);

  }


  private void DoMovement(InputData input, float delta_time)
  {
    float camera_speed = 0.02f;

    if (input.Keys[(int)Keys.W])
    {
      _pos += _direction * camera_speed * delta_time;
    }
    if (input.Keys[(int)Keys.S])
    {
      _pos -= _direction * camera_speed * delta_time;
    }
    if (input.Keys[(int)Keys.D])
    {
      _pos += Vector3.Cross(_direction, _up) * camera_speed * delta_time;
    }
    if (input.Keys[(int)Keys.A])
    {
      _pos += Vector3.Cross(_up, _direction) * camera_speed * delta_time;
    }
  }

  private void DoRotating(InputData input, float delta_time)
  {
    float sensitivity = 0.04f;
    _pitch -= input.DeltaMousePosition.Y * sensitivity;
    _yaw += input.DeltaMousePosition.X * sensitivity;

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

}