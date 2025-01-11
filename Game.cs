using LearnOpenGL.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenGL;

public class Game : GameWindow
{
    private Lighting_Light _lLight = null!;
    private Lighting_Cube _lCube = null!;

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

        var cameraPosition = new Vector3(0.0f, 0.0f, 5.0f);
        var cameraTarget = new Vector3(0.0f, 0.0f, 1.0f);
        var cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        _camera = new Camera(cameraPosition, cameraTarget, cameraUp, FramebufferSize.X / (float)FramebufferSize.Y);
        _view = _camera.LookAt();
        _projection = _camera.CreatePerspectiveFieldOfView();
        
        var lightColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        var cubeColor = new Vector4(1.0f, 0.5f, 0.3f, 1.0f);
        _lLight = new Lighting_Light(_view, _projection);
        _lLight.SetObjectColor(lightColor);
        _lCube = new Lighting_Cube(_view, _projection);
        _lCube.SetObjectColor(cubeColor);
        _lCube.SetLightColor(lightColor);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _view = _camera.LookAt();
        _projection = _camera.CreatePerspectiveFieldOfView();

        _lCube.Draw(_view, _projection, args.Time);
        _lLight.Draw(_view, _projection, args.Time);

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

        _lCube.Dispose();
        _lLight.Dispose();
    }
}
