#version 330 core
in vec3 aPosition;
in vec2 aTexture;

out vec2 textureCoordinate;

void main()
{
    gl_Position = vec4(aPosition, 1.0);

    textureCoordinate = aTexture;
}
