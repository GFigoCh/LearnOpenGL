using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenGL;

public class Game : GameWindow
{
    private Shader _gsShader = null!;
    private Texture _gsTexture = null!;
    private Texture _gsTextureLayer = null!;
    private int _gsVertexArrayObject = 0;
    private Matrix4 _gsModel;

    private Shader _lCubeShader = null!;
    private int _lCubeVertexArrayObject = 0;
    private Matrix4 _lCubeModel;
    private Matrix3 _lCubeNModel;

    private Shader _lLightShader = null!;
    private int _lLightVertexArrayObject = 0;
    private Matrix4 _lLightModel;

    private int _vertexBufferObject = 0;
    private float[] _vertices = {
        // Position             // Normal               // Texture
        
        // Front
        -1.0f, -1.0f, 1.0f,     0.0f, 0.0f, 1.0f,       0.0f, 0.0f,
        1.0f, -1.0f, 1.0f,      0.0f, 0.0f, 1.0f,       1.0f, 0.0f,
        1.0f, 1.0f, 1.0f,       0.0f, 0.0f, 1.0f,       1.0f, 1.0f,
        -1.0f, 1.0f, 1.0f,      0.0f, 0.0f, 1.0f,       0.0f, 1.0f,

        // Back
        -1.0f, -1.0f, -1.0f,    0.0f, 0.0f, -1.0f,      0.0f, 0.0f,
        1.0f, -1.0f, -1.0f,     0.0f, 0.0f, -1.0f,      1.0f, 0.0f,
        1.0f, 1.0f, -1.0f,      0.0f, 0.0f, -1.0f,      1.0f, 1.0f,
        -1.0f, 1.0f, -1.0f,     0.0f, 0.0f, -1.0f,      0.0f, 1.0f,

        // Top
        -1.0f, 1.0f, -1.0f,     0.0f, 1.0f, 0.0f,       0.0f, 0.0f,
        1.0f, 1.0f, -1.0f,      0.0f, 1.0f, 0.0f,       1.0f, 0.0f,
        1.0f, 1.0f, 1.0f,       0.0f, 1.0f, 0.0f,       1.0f, 1.0f,
        -1.0f, 1.0f, 1.0f,      0.0f, 1.0f, 0.0f,       0.0f, 1.0f,

        // Left
        -1.0f, -1.0f, 1.0f,     -1.0f, 0.0f, 0.0f,      0.0f, 0.0f,
        -1.0f, -1.0f, -1.0f,    -1.0f, 0.0f, 0.0f,      1.0f, 0.0f,
        -1.0f, 1.0f, -1.0f,     -1.0f, 0.0f, 0.0f,      1.0f, 1.0f,
        -1.0f, 1.0f, 1.0f,      -1.0f, 0.0f, 0.0f,      0.0f, 1.0f,

        // Right
        1.0f, -1.0f, 1.0f,      1.0f, 0.0f, 0.0f,       0.0f, 0.0f,
        1.0f, -1.0f, -1.0f,     1.0f, 0.0f, 0.0f,       1.0f, 0.0f,
        1.0f, 1.0f, -1.0f,      1.0f, 0.0f, 0.0f,       1.0f, 1.0f,
        1.0f, 1.0f, 1.0f,       1.0f, 0.0f, 0.0f,       0.0f, 1.0f,

        // Bottom
        -1.0f, -1.0f, -1.0f,    0.0f, -1.0f, 0.0f,      0.0f, 0.0f,
        1.0f, -1.0f, -1.0f,     0.0f, -1.0f, 0.0f,      1.0f, 0.0f,
        1.0f, -1.0f, 1.0f,      0.0f, -1.0f, 0.0f,      1.0f, 1.0f,
        -1.0f, -1.0f, 1.0f,     0.0f, -1.0f, 0.0f,      0.0f, 1.0f,
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

    private Matrix4 _view;
    private Matrix4 _projection;
    private Camera _camera = null!;

    public Game(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) {}

    protected override void OnLoad()
    {
        base.OnLoad();
        
        CursorState = CursorState.Grabbed;
        
        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        var cameraPosition = new Vector3(0.0f, 0.0f, 10.0f);
        var cameraTarget = new Vector3(0.0f, 0.0f, 1.0f);
        var cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        _camera = new Camera(cameraPosition, cameraTarget, cameraUp, FramebufferSize.X / (float)FramebufferSize.Y);

        #region Getting Started Cube Init
        _gsShader = new Shader("Shaders/gs_cube.vert", "Shaders/gs_cube.frag");
        _gsShader.Use();

        _gsTexture = new Texture(TextureUnit.Texture0, "Textures/container_wood.jpg");
        _gsTextureLayer = new Texture(TextureUnit.Texture1, "Textures/leaf_corners.png");
        _gsShader.SetUniform1Int("texture0", 0);
        _gsShader.SetUniform1Int("texture1", 1);

        _gsVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_gsVertexArrayObject);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

        int aPositionGS = _gsShader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(aPositionGS, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPositionGS);

        int aTextureGS = _gsShader.GetAttribLocation("aTexture");
        GL.VertexAttribPointer(aTextureGS, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(aTextureGS);

        _gsModel = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45.0f));
        _gsModel *= Matrix4.CreateTranslation(0.0f, 0.0f, -10.0f);
        _gsShader.SetUniformMatrix4("model", ref _gsModel);
        #endregion
        
        var cubeColor = new Vector3(1.0f, 0.5f, 0.3f);
        var lightColor = new Vector3(1.0f, 1.0f, 1.0f);
        var lightPosition = new Vector3(10.0f, 5.0f, 10.0f);

        // Lighting Cube Init
        _lCubeShader = new Shader("Shaders/l_cube.vert", "Shaders/l_cube.frag");
        _lCubeShader.Use();

        _lCubeVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_lCubeVertexArrayObject);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

        int aPositionLCube = _lCubeShader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(aPositionLCube, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPositionLCube);

        int aNormalLCube = _lCubeShader.GetAttribLocation("aNormal");
        GL.VertexAttribPointer(aNormalLCube, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(aNormalLCube);

        _lCubeModel = Matrix4.CreateScale(2.0f);
        _lCubeShader.SetUniformMatrix4("model", ref _lCubeModel);
        _lCubeNModel = Matrix3.Transpose(Matrix3.Invert(new Matrix3(_lCubeModel)));
        _lCubeShader.SetUniformMatrix3("nModel", ref _lCubeNModel);

        _lCubeShader.SetUniform3("objectColor", cubeColor);
        _lCubeShader.SetUniform3("lightColor", lightColor);
        _lCubeShader.SetUniform3("lightPosition", lightPosition);

        // Lighting Light Init
        _lLightShader = new Shader("Shaders/l_light.vert", "Shaders/l_light.frag");
        _lLightShader.Use();

        _lLightVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_lLightVertexArrayObject);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

        int aPositionLLight = _lLightShader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(aPositionLLight, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPositionLLight);

        _lLightModel = Matrix4.CreateScale(0.25f);
        _lLightModel *= Matrix4.CreateTranslation(lightPosition);
        _lLightShader.SetUniformMatrix4("model", ref _lLightModel);

        _lLightShader.SetUniform3("objectColor", lightColor);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _view = _camera.LookAt();
        _projection = _camera.CreatePerspectiveFieldOfView();

        #region Getting Started Cube Draw
        _gsShader.Use();
        _gsTexture.Use(TextureUnit.Texture0);
        _gsTextureLayer.Use(TextureUnit.Texture1);
        GL.BindVertexArray(_gsVertexArrayObject);

        _gsShader.SetUniformMatrix4("view", ref _view);
        _gsShader.SetUniformMatrix4("projection", ref _projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        #endregion

        // Lighting Cube Draw
        _lCubeShader.Use();
        GL.BindVertexArray(_lCubeVertexArrayObject);

        _lCubeShader.SetUniformMatrix4("view", ref _view);
        _lCubeShader.SetUniformMatrix4("projection", ref _projection);

        _lCubeShader.SetUniform3("viewPosition", _camera.Position);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        // Lighting Light Draw
        _lLightShader.Use();
        GL.BindVertexArray(_lLightVertexArrayObject);

        _lLightShader.SetUniformMatrix4("view", ref _view);
        _lLightShader.SetUniformMatrix4("projection", ref _projection);

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

        _gsShader.Dispose();
        _lCubeShader.Dispose();
        _lLightShader.Dispose();
    }
}
