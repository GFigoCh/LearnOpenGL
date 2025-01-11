#version 330 core
in vec3 aPosition;
in vec2 aTexture;

out vec2 textureCoordinate;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);

    textureCoordinate = aTexture;
}
