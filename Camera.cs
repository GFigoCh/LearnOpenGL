using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenGL;

public class Camera
{
    private float _speed;
    private Vector3 _position;
    public Vector3 Position => _position;
    private Vector3 _target;
    private Vector3 _up;
    private Vector3 _right;

    private float _sensitivity;
    private float _pitch;
    private float _yaw;

    private float _fov;
    private float _aspectRatio;
    private float _depthNear;
    private float _depthFar;

    public Camera(Vector3 position, Vector3 target, Vector3 up, float aspectRatio)
    {
        _speed = 5.0f;
        _position = position;
        _target = target;
        _up = up;
        _right = Vector3.Normalize(Vector3.Cross(_up, _target));

        _sensitivity = 0.1f;
        _pitch = 0.0f;
        _yaw = 90.0f;

        _fov = 45.0f;
        _aspectRatio = aspectRatio;
        _depthNear = 0.1f;
        _depthFar = 100.0f;
    }

    public Matrix4 LookAt()
    {
        return Matrix4.LookAt(_position, _position - _target, _up);
    }

    public Matrix4 CreatePerspectiveFieldOfView()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), _aspectRatio, _depthNear, _depthFar);
    }

    public void KeyboardHandler(KeyboardState state, double deltaTime)
    {
        if (state.IsKeyDown(Keys.W))
        {
            _position -= _target * _speed * (float)deltaTime;
        }

        if (state.IsKeyDown(Keys.S))
        {
            _position += _target * _speed * (float)deltaTime;
        }

        if (state.IsKeyDown(Keys.A))
        {
            _position -= _right * _speed * (float)deltaTime;
        }

        if (state.IsKeyDown(Keys.D))
        {
            _position += _right * _speed * (float)deltaTime;
        }

        if (state.IsKeyDown(Keys.LeftShift))
        {
            _position -= _up * _speed * (float)deltaTime;
        }

        if (state.IsKeyDown(Keys.Space))
        {
            _position += _up * _speed * (float)deltaTime;
        }
    }

    public void MouseMoveHandler(float deltaX, float deltaY)
    {
        _yaw += deltaX * _sensitivity;
        _pitch = Math.Clamp(_pitch + deltaY * _sensitivity, -89.0f, 89.0f);
        
        float yawRadians = MathHelper.DegreesToRadians(_yaw);
        float pitchRadians = MathHelper.DegreesToRadians(_pitch);
        _target.X = (float)(Math.Cos(pitchRadians) * Math.Cos(yawRadians));
        _target.Y = (float)Math.Sin(pitchRadians);
        _target.Z = (float)(Math.Cos(pitchRadians) * Math.Sin(yawRadians));

        _right = Vector3.Normalize(Vector3.Cross(_up, _target));
    }

    public void MouseWheelHandler(float offsetY)
    {
        _fov = Math.Clamp(_fov - offsetY, 1.0f, 180.0f);
    }

    public void FramebufferResizeHandler(int width, int height)
    {
        _aspectRatio = width / (float)height;
    }
}
