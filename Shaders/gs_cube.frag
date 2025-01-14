#version 330 core
in vec2 textureCoordinate;

out vec4 fragmentColor;

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    fragmentColor = mix(texture(texture0, textureCoordinate), texture(texture1, textureCoordinate), 0.25);
}
