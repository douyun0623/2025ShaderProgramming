#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_UV;

uniform sampler2D u_RGBTexture;

uniform float u_Time;       // 시간(초 단위)

const float c_PI = 3.14159265;

void Test()
{
    vec2 newUV = v_UV;
    float dx = 0.1f * cos(v_UV.x * 2 * c_PI + u_Time);
    float dy = 0.1f * sin(v_UV.x * 2 * c_PI + u_Time);
    newUV += vec2(dx, dy);
    vec4 sampledColor = texture(u_RGBTexture, newUV);
    FragColor = sampledColor;
}

void Circless()
{
    vec2 newUV = v_UV;
    vec2 center = vec2(0.5, 0.5);
    vec4 newColor = vec4(0);
    float d = distance(newUV, center);
    
    float value = sin(d * 8 * c_PI - u_Time);
    newColor = vec4(value);

    FragColor = newColor;

}

void Flag()
{
    vec2 newUV = vec2(v_UV.x, 1 - v_UV.y - 0.5);  // 0-1, left bottom (0,0)
    vec4 newColor = vec4(0);

    // float width = 0.2f - v_UV.x * 0.2;
    float width = 0.2f * (1 - newUV.x);
    float sinValue = v_UV.x * 0.2 * sin(newUV.x * 2 * c_PI - u_Time * 10);

    if(newUV.y < sinValue + width && newUV.y > sinValue - width && width > 0)
    {
        newColor = vec4(1);
    }
    else{
        discard;
    }
    FragColor = newColor;
}

// 좌표 꼬아보기
// 수식을 사용하여 if문 없이 발표자료 1을 결과를 만들어보자
void Q1()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);  // 0-1, left top (0,0)
    float x = newUV.x;  // 0-1
    float y = 1 - abs((v_UV.y - 0.5) * 2);  // -0.5 - 0.5 => -1~1 => 1~0~1 => 0~1~0
    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    FragColor = newColor;
}


void Q2()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);  // 0-1, left top (0,0)
    float x = fract(newUV.x * 3);  // 0-1 (3)
    float y = (2 - floor(newUV.x * 3))/3 + newUV.y / 3; // 
    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    FragColor = newColor;
}

void Q3()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);  // 0-1, left top (0,0)
    float x = fract(newUV.x * 3);  // 0-1 (3)
    float y = (floor(newUV.x * 3))/3 + newUV.y / 3; // 
    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    FragColor = newColor;
}

void Q4()
{
    vec2 newUV = vec2(v_UV.x, v_UV.y);  // 0-1, left top (0,0)
    float value = floor(newUV.y * 2) / 2 + 0.5;   // 0, 1 => 0.5 1
    float x = fract(newUV.x * 2 + value);  // 0-1 (3)
    float y = newUV.y * 2; // 
    vec4 newColor = texture(u_RGBTexture, vec2(x,y));

    FragColor = newColor;
}

void main()
{
    // Test();
    // Circless();
    // Flag();
    // Q1();
    // Q2();
    Q3();
    // Q4();
}