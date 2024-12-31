#version 330 core
in vec2 textureCoordinate;

out vec4 fragmentColor;

uniform sampler2D texture0;

void main()
{
    fragmentColor = texture(texture0, textureCoordinate);
}
