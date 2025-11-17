#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_UV;

uniform float u_Time;

const float c_PI = 3.14159265;

void main()
{
    // 1. UV 좌표를 화면 중앙(0.0, 0.0) 기준으로 변경
    vec2 uv = (v_UV - 0.5) * 2.0;

    // 2. 직교 좌표(uv)를 극좌표(r, a)로 변환
    float r = max(length(uv), 0.0001); // 0 나누기 방지
    float a = atan(uv.y, uv.x);

    // 3. 줌 필드 (중심으로 빨려 들어가는 효과)
    //    log(r)을 이용해 거리를 반복적인 스케일로 변환
    //    u_Time을 더해 패턴이 움직이도록 함
    float zoom_field = log(r) * 3.0 - u_Time * 2.0; // 링의 촘촘함과 줌 속도 조절

    // 4. (이미지 유사 패턴 생성) 복잡도 높은 패턴 만들기
    //    여러 겹의 sin/cos 함수를 중첩하고 for 루프를 사용해 반복
    float total_pattern = 0.0;
    
    // (A) 기본 대칭 패턴 (이미지의 큰 팔들을 만듭니다)
    //    a * N.0 : N개의 팔을 가진 패턴
    //    cos(zoom_field * M.0) : 줌에 따라 나타나는 링 패턴
    total_pattern += sin(a * 7.0 + cos(zoom_field * 0.5)); // 7개 팔
    total_pattern += cos(a * 11.0 + sin(zoom_field * 0.8)); // 11개 팔
    total_pattern += sin(a * 4.0 + zoom_field * 1.5); // 4개 팔 (더 빠르게 변화)

    // (B) (이미지의 핵심) 반복적인 패턴 중첩
    //     r과 a를 `for` 루프 안에서 변형시켜 프랙탈 같은 복잡도 부여
    vec2 current_uv = uv;
    float current_r = r;
    float current_a = a;

    const int ITERATIONS = 5; // 패턴 중첩 횟수 (이미지 복잡도)
    for (int i = 0; i < ITERATIONS; i++) {
        // 좌표를 접고 회전, 스케일링하여 반복적인 구조 생성 (만화경 프랙탈과 유사)
        current_uv = abs(current_uv);
        current_uv -= 0.2; // 중앙으로 조금 당김
        current_uv *= 1.5; // 확대 (다음 반복을 위해)
        
        // 각도도 변형하여 나선형/회전하는 복잡한 패턴 추가
        current_a = atan(current_uv.y, current_uv.x);
        current_r = length(current_uv);

        // 이 새로운 (변형된) r과 a를 이용해 패턴을 중첩
        // zoom_field 값도 다시 계산하여 줌인 효과가 겹쳐지도록 함
        float inner_zoom_field = log(max(current_r, 0.0001)) * 3.0 - u_Time * 2.0;

        total_pattern += sin(current_a * (float(i) * 2.0 + 3.0) + inner_zoom_field * 0.7);
        total_pattern += cos(current_a * (float(i) * 3.0 + 5.0) + inner_zoom_field * 1.2);
    }

    // 최종 패턴 값을 0.0 ~ 1.0 범위로 정규화
    // (더 복잡해졌으므로 정규화 범위 조절)
    total_pattern = fract(total_pattern * 0.1); // fract로 값을 0-1 사이로 다시 접어줌

    // 5. (이미지 유사 네온 효과) 날카로운 경계와 발광
    //    smoothstep의 범위를 좁게 하여 날카로운 네온 라인 생성
    float neon_line_width = 0.05; // 네온 라인 폭 조절
    float neon_glow = smoothstep(0.5 - neon_line_width, 0.5, total_pattern) 
                    - smoothstep(0.5, 0.5 + neon_line_width, total_pattern);
    
    // 발광 강도 (이미지처럼 강하게)
    neon_glow = pow(neon_glow, 1.0) * 4.0; // pow 지수 낮춰서 좀 더 굵게

    // 6. (이미지 유사 네온 컬러) 시간과 패턴에 따라 변하는 화려한 색상 스펙트럼
    vec3 neon_color = 0.5 + 0.5 * cos(
        c_PI * (total_pattern * 5.0 + u_Time * 0.5) + vec3(0.0, 0.33, 0.66) 
    );
    
    // 7. 최종 색상 (배경은 어둡게, 네온은 밝게)
    vec3 finalColor = neon_color * neon_glow; 

    // 8. (이미지 유사) 중앙 블랙홀 효과 (smoothstep으로 부드럽게 전환)
    finalColor *= smoothstep(0.0, 0.1, r); 
    
    // 9. (이미지 유사) 가장자리 비네팅 효과 (패턴을 덮지 않도록 부드럽게)
    finalColor *= (1.0 - pow(r, 3.0) * 0.4); // pow 지수와 곱하는 값 조절

    FragColor = vec4(finalColor, 1.0);
}