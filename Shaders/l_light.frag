#version 330 core
out vec4 fragmentColor;

uniform vec3 objectColor;

void main()
{
    fragmentColor = vec4(objectColor, 1.0);
}
