#version 330 core
out vec4 fragmentColor;

uniform vec4 objectColor;
uniform vec4 lightColor;

void main()
{
    fragmentColor = objectColor * lightColor;
}
