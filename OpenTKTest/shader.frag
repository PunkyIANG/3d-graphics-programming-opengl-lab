﻿#version 330 core
out vec4 FragColor;

uniform mat4 transform;

void main()
{
    FragColor = vec4(transform);
//    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}
