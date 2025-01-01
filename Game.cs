using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenGL;

public class Game : GameWindow
{
    private Shader _shader = null!;
    private Texture _texture = null!;
    private Texture _textureLayer = null!;
    private int _vertexArrayObject = 0;
    private int _vertexBufferObject = 0;
    private float[] _vertices = {
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f
    };
    private int _elementBufferObject = 0;
    private uint[] _indices = {
        0, 1, 2,
        2, 3, 0
    };

    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default,
            new NativeWindowSettings() {ClientSize = (width, height), Title = title}) {}

    protected override void OnLoad()
    {
        base.OnLoad();
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
        _shader.SetTextureSampler("texture0", 0);
        _shader.SetTextureSampler("texture1", 1);

        _texture = new Texture("Textures/container_wood.jpg");
        _textureLayer = new Texture("Textures/leaf_corners.png");
        _texture.Use();
        _textureLayer.Use(TextureUnit.Texture1);

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

        TransformTest();
    }

    private void TransformTest()
    {
        var translation = Matrix4.CreateTranslation(1.0f, 1.0f, 0.0f);
        var rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
        var scale = Matrix4.CreateScale(0.5f, 0.5f, 0.0f);
        Matrix4 transform = translation * rotation * scale;

        int transformLocation = _shader.GetUniformLocation("transform");
        GL.UniformMatrix4(transformLocation, true, ref transform);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        base.OnFramebufferResize(args);

        GL.Viewport(0, 0, args.Width, args.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _shader.Dispose();
    }
}
