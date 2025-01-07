using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenGL;

public class Camera
{
    private float _speed;
    private Vector3 _position;
    private Vector3 _target;
    private Vector3 _up;
    private Vector3 _right;

    private float _sensitivity;
    private float _pitch;
    private float _yaw;

    public Camera(Vector3 position, Vector3 target, Vector3 up)
    {
        _speed = 1.0f;
        
        _position = position;
        _target = target;
        _up = up;
        
        _right = Vector3.Normalize(Vector3.Cross(_up, _target));

        _sensitivity = 0.1f;
        _pitch = 0.0f;
        _yaw = 90.0f;
    }

    public Matrix4 LookAt()
    {
        return Matrix4.LookAt(_position, _position - _target, _up);
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

    public void MouseHandler(MouseState state)
    {
        _yaw += state.Delta.X * _sensitivity;
        _pitch = Math.Clamp(_pitch + state.Delta.Y * _sensitivity, -89.0f, 89.0f);
        
        float yawRadians = MathHelper.DegreesToRadians(_yaw);
        float pitchRadians = MathHelper.DegreesToRadians(_pitch);
        _target.X = (float)(Math.Cos(pitchRadians) * Math.Cos(yawRadians));
        _target.Y = (float)Math.Sin(pitchRadians);
        _target.Z = (float)(Math.Cos(pitchRadians) * Math.Sin(yawRadians));

        _right = Vector3.Normalize(Vector3.Cross(_up, _target));
    }
}
