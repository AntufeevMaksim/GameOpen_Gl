#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec3 ourColor;

uniform sampler2D texture0;
uniform sampler2D texture1;

uniform float mix_coefficient;

void main()
{
//    outputColor = texture(texture0, texCoord) * vec4(ourColor, 1.0);
//    outputColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);
    outputColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), mix_coefficient);
}