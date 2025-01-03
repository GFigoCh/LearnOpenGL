using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace LearnOpenGL;

public class Texture
{
    private int _handle = 0;

    public Texture(TextureUnit unit, string texturePath)
    {
        _handle = GL.GenTexture();
        Use(unit);

        StbImage.stbi_set_flip_vertically_on_load(1);

        byte[] textureBuffer = File.ReadAllBytes(texturePath);
        ImageResult texture = ImageResult.FromMemory(textureBuffer, ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}
