using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LearnOpenGL.Objects;

public class GettingStarted_Cube : IDisposable
{
    private Shader _shader;
    private Texture _texture;
    private Texture _textureLayer;
    private int _vertexArrayObject = 0;
    private int _vertexBufferObject = 0;
    private float[] _vertices = {
        // Front
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,

        // Back
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,

        // Top
        -0.5f, 0.5f, -0.5f, 0.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,

        // Left
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,

        // Right
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f,

        // Bottom
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 1.0f,
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
        8, 9, 10,
        10, 11, 8,

        // Left
        12, 13, 14,
        14, 15, 12,

        // Right
        16, 17, 18,
        18, 19, 16,

        // Bottom
        20, 21, 22,
        22, 23, 20,
    };
    private Matrix4 _model;
    private bool _disposedValue = false;

    public GettingStarted_Cube(Matrix4 view, Matrix4 projection)
    {
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();

        _texture = new Texture(TextureUnit.Texture0, "Textures/container_wood.jpg");
        _textureLayer = new Texture(TextureUnit.Texture1, "Textures/leaf_corners.png");
        _shader.SetTextureSampler("texture0", 0);
        _shader.SetTextureSampler("texture1", 1);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        int aPositionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(aPositionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPositionLocation);

        int aTextureLocation = _shader.GetAttribLocation("aTexture");
        GL.VertexAttribPointer(aTextureLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(aTextureLocation);

        _model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45.0f));
        _shader.SetCoordinateSystem("model", ref _model);

        _shader.SetCoordinateSystem("view", ref view);
        _shader.SetCoordinateSystem("projection", ref projection);
    }

    public void Draw(Matrix4 view, Matrix4 projection, double deltaTime)
    {
        // _shader.Use();
        // _texture.Use(TextureUnit.Texture0);
        // _textureLayer.Use(TextureUnit.Texture1);
        // GL.BindVertexArray(_vertexArrayObject);

        _model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)deltaTime * 45.0f));
        _shader.SetCoordinateSystem("model", ref _model);

        _shader.SetCoordinateSystem("view", ref view);
        _shader.SetCoordinateSystem("projection", ref projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
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
    ~GettingStarted_Cube()
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
