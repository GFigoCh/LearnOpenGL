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
    private Matrix4 _view;
    private Matrix4 _projection;
    private Camera _camera = null!;

    public Game(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) {}

    protected override void OnLoad()
    {
        base.OnLoad();
        
        CursorState = CursorState.Grabbed;
        
        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
        _shader.SetTextureSampler("texture0", 0);
        _shader.SetTextureSampler("texture1", 1);

        _texture = new Texture(TextureUnit.Texture0, "Textures/container_wood.jpg");
        _textureLayer = new Texture(TextureUnit.Texture1, "Textures/leaf_corners.png");

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

        var cameraPosition = new Vector3(0.0f, 0.0f, 3.0f);
        var cameraTarget = new Vector3(0.0f, 0.0f, 1.0f);
        var cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        _camera = new Camera(cameraPosition, cameraTarget, cameraUp, FramebufferSize.X / (float)FramebufferSize.Y);
        _view = _camera.LookAt();
        _shader.SetCoordinateSystem("view", ref _view);
        _projection = _camera.CreatePerspectiveFieldOfView();
        _shader.SetCoordinateSystem("projection", ref _projection);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)args.Time * 45.0f));
        _shader.SetCoordinateSystem("model", ref _model);

        _view = _camera.LookAt();
        _shader.SetCoordinateSystem("view", ref _view);
        _projection = _camera.CreatePerspectiveFieldOfView();
        _shader.SetCoordinateSystem("projection", ref _projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        _camera.KeyboardHandler(KeyboardState, args.Time);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs args)
    {
        base.OnKeyDown(args);
        
        switch (args.Key)
        {
            case Keys.Escape:
                Close();
                break;
        }
    }

    protected override void OnMouseMove(MouseMoveEventArgs args)
    {
        base.OnMouseMove(args);

        _camera.MouseMoveHandler(args.DeltaX, args.DeltaY);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs args)
    {
        base.OnMouseWheel(args);

        _camera.MouseWheelHandler(args.OffsetY);
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        base.OnFramebufferResize(args);

        GL.Viewport(0, 0, args.Width, args.Height);

        _camera.FramebufferResizeHandler(args.Width, args.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _shader.Dispose();
    }
}
