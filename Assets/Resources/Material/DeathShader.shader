Shader "Custom/DeathShader"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _SubTex ("_SubTex", 2D) = "white" {}
        _Duration ("_Duration", Float) = 3.0
        _Width ("_Width", Float) = 0.1
        _GeneratedTime ("_GeneratedTime", Float) = 0.0
    }
    SubShader
    {
        // 반투명
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        
        Pass
        {
            // Premultiplied transparency
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            sampler2D _SubTex;
            float _Duration;
            float _GeneratedTime;
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                // timebase로 uv.y가 일정 이상이면 color.a *= sub texture color.a
                // sub texture에 의해 그려지는 픽셀은 timebase로 위로 상승하면서 딤드
                // uv.y가 일정 이하인 부분은 x로도 축소

                // 1~0
                fixed fixedTime = 1.0 - fmod(_Time.y - _GeneratedTime, _Duration) / _Duration;

                fixed progress = fixedTime - i.uv.y; // 위에서 아래로 제거됨
                progress *= 2.0; // 제거되지 않았을 때의 컬러 2배

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 subCol = tex2D(_SubTex, i.uv);

                // TODO
                progress = progress + (progress + 1.0) * subCol.r * 3.0 - 0.5;

                progress = max(min(progress, 1.0), 0.0);

                col.rgb *= col.a; // TODO 왠진몰라도 alpha 적용이안돼서 수동으로 적용
                col.a = 0.0;

                col.rgb *= progress;
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
