#version 330 core
layout (location = 0) in vec3 aPosition;

out vec3 currentPos;

void main()
{
    currentPos = aPosition;
    gl_Position = vec4(aPosition, 1.0);
}
