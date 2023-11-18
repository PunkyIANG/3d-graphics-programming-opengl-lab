#version 330 core
out vec4 FragColor;

uniform vec4 firstColor;
uniform vec4 secondColor;

uniform vec3 firstPos;
uniform vec3 secondPos;
in vec3 currentPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vec3 modFirstPos = vec3(vec4(firstPos, 1.0) * model * view * projection);
    vec3 modSecondPos = vec3(vec4(secondPos, 1.0) * model * view * projection);
    
    float topSide = (modSecondPos.x - modFirstPos.x) * (modFirstPos.x - currentPos.x)
    + (modSecondPos.y - modFirstPos.y) * (modFirstPos.y - currentPos.y)
    + (modSecondPos.z - modFirstPos.z) * (modFirstPos.z - currentPos.z);

    float bottomSide = pow(modSecondPos.x - modFirstPos.x, 2)
    + pow(modSecondPos.y - modFirstPos.y, 2)
    + pow(modSecondPos.z - modFirstPos.z, 2);

    float t = clamp(-topSide / bottomSide, 0, 1);

    FragColor = mix(firstColor, secondColor, t);
}
