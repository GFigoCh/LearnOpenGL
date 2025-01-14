#version 330 core
in vec3 normal;
in vec3 fragmentPosition;

out vec4 fragmentColor;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPosition;
uniform vec3 viewPosition;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 uNormal = normalize(normal);
    vec3 lightDirection = normalize(lightPosition - fragmentPosition);
    float dImpact = max(dot(uNormal, lightDirection), 0.0);
    vec3 diffuse = dImpact * lightColor;

    float specularStrength = 0.5;
    vec3 viewDirection = normalize(viewPosition - fragmentPosition);
    vec3 reflectDirection = reflect(-lightDirection, uNormal);
    float sImpact = pow(max(dot(viewDirection, reflectDirection), 0.0), 32);
    vec3 specular = specularStrength * sImpact * lightColor;

    vec3 result = objectColor * (ambient + diffuse + specular);
    fragmentColor = vec4(result, 1.0);
}
