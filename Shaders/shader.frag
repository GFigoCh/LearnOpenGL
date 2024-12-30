#version 330 core
in vec4 vertexColor;

out vec4 fragmentColor;

// uniform vec4 globalColor;

void main()
{
    fragmentColor = vertexColor;
    // fragmentColor = globalColor;
}
