#version 330 core
in vec3 aPosition;
in vec3 aNormal;

out vec3 normal;
out vec3 fragmentPosition;

uniform mat4 model;
uniform mat3 mNormal;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vec4 position = vec4(aPosition, 1.0);

    gl_Position = projection * view * model * position;

    fragmentPosition = vec3(model * position);
    normal = mNormal * aNormal;
}
