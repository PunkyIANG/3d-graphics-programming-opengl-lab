#version 330 core

layout(location = 0) in vec3 aPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 currentPos;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    currentPos = vec3(vec4(aPosition, 1.0) * model * view * projection);
}