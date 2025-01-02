using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LearnOpenGL;

public class Shader : IDisposable
{
    private int _handle = 0;
    private bool _disposedValue = false;

    public Shader(string vertexPath, string fragmentPath)
    {
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        string vertexShaderSource = File.ReadAllText(vertexPath);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        CompileShader(fragmentShader);

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            Console.WriteLine(GL.GetProgramInfoLog(_handle));
        }

        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(fragmentShader);

        GL.DetachShader(_handle, vertexShader);
        GL.DeleteShader(vertexShader);
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0)
        {
            Console.WriteLine(GL.GetShaderInfoLog(shader));
        }
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(_handle, attribName);
    }

    public int GetUniformLocation(string uniformName)
    {
        return GL.GetUniformLocation(_handle, uniformName);
    }

    public void SetTextureSampler(string samplerName, int index)
    {
        GL.Uniform1(GetUniformLocation(samplerName), index);
    }

    public void SetCoordinateSystem(string name, ref Matrix4 matrix)
    {
        GL.UniformMatrix4(GetUniformLocation(name), false, ref matrix);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
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
            GL.DeleteProgram(_handle);
            _disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~Shader()
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
