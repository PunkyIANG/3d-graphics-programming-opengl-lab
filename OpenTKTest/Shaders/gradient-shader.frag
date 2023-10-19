#version 330 core
out vec4 FragColor;

uniform vec4 firstColor;
uniform vec4 secondColor;

uniform vec3 firstPos;
uniform vec3 secondPos;
in vec3 currentPos;

void main()
{
    float topSide = (secondPos.x - firstPos.x) * (firstPos.x - currentPos.x)
                  + (secondPos.y - firstPos.y) * (firstPos.y - currentPos.y)
                  + (secondPos.z - firstPos.z) * (firstPos.z - currentPos.z);
    
    float bottomSide = pow(secondPos.x - firstPos.x, 2)
            + pow(secondPos.y - firstPos.y, 2)
            + pow(secondPos.z - firstPos.z, 2);

    float t = clamp(-topSide / bottomSide, 0, 1);
    
    FragColor = mix(firstColor, secondColor, t);
}
