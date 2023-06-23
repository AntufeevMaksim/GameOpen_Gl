#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aOurColor;

out vec2 texCoord;
out vec3 ourColor;

uniform mat4 transform;


void main(void)
{
    texCoord = aTexCoord;
    ourColor = aOurColor;
    
    gl_Position = vec4(aPosition, 1.0) * transform;
}