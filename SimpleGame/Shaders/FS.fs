#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_UV;

uniform sampler2D u_RGBTexture;

uniform float u_Time;       // 시간(초 단위)

const float c_PI = 3.14159265;

void main()
{
    vec2 newUV = v_UV;
    float dx = 0.1f * cos(v_UV.x * 2 * c_PI + u_Time);
    float dy = 0.1f * sin(v_UV.x * 2 * c_PI + u_Time);
    newUV += vec2(dx, dy);
    vec4 sampledColor = texture(u_RGBTexture, newUV);
    FragColor = sampledColor;
}