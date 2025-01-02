#version 330 core
in vec3 aPosition;
in vec2 aTexture;

out vec2 textureCoordinate;

uniform mat4 transform;

void main()
{
    gl_Position = transform * vec4(aPosition, 1.0);

    textureCoordinate = aTexture;
}
