using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LearnOpenGL.Objects;

public class Lighting_Light : IDisposable
{
    private Shader _shader;
    private int _vertexArrayObject = 0;
    private int _vertexBufferObject = 0;
    private float[] _vertices = {
        // Front
        -0.5f, -0.5f, 0.5f,
        0.5f, -0.5f, 0.5f,
        0.5f, 0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f,

        // Back
        -0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f, 0.5f, -0.5f,
        -0.5f, 0.5f, -0.5f,
    };
    private int _elementBufferObject = 0;
    private uint[] _indices = {
        // Front
        0, 1, 2,
        2, 3, 0,

        // Back
        4, 5, 6,
        6, 7, 4,

        // Top
        3, 2, 6,
        6, 7, 3,

        // Bottom
        0, 1, 5,
        5, 4, 0,

        // Left
        0, 4, 7,
        7, 3, 0,

        // Right
        1, 5, 6,
        6, 2, 1,
    };
    private Matrix4 _model;
    private bool _disposedValue = false;

    public Lighting_Light()
    {
        _shader = new Shader("Shaders/l_light.vert", "Shaders/l_light.frag");
        _shader.Use();

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        int aPositionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(aPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPositionLocation);

        _model = Matrix4.CreateScale(0.25f);
        _shader.SetUniformMatrix4("model", ref _model);
    }

    public void Draw(Matrix4 view, Matrix4 projection, double deltaTime)
    {
        _shader.Use();
        GL.BindVertexArray(_vertexArrayObject);

        _shader.SetUniformMatrix4("view", ref view);
        _shader.SetUniformMatrix4("projection", ref projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void SetObjectColor(Vector3 vector)
    {
        _shader.SetUniform3("objectColor", vector);
    }

    public void SetObjectPosition(Vector3 vector)
    {
        var model = _model * Matrix4.CreateTranslation(vector);
        _shader.SetUniformMatrix4("model", ref model);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            // if (disposing)
            // {
            //     // TODO: dispose managed state (managed objects)
            // }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_elementBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            _shader.Dispose();
            _disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~Lighting_Light()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
