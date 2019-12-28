Shader "Custom/DeathShader"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _SubTex ("_SubTex", 2D) = "white" {}
        _Duration ("_Duration", Float) = 1.75
        _DirectionX ("_DirectionX", Range (-1.0, 1.0)) = 1.0
        _DirectionY ("_DirectionY", Range (-1.0, 1.0)) = 0.0
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
            float _DirectionX;
            float _DirectionY;
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
                // t = 0~1
                fixed t = fmod(_Time.y - _GeneratedTime, _Duration) / _Duration;

                fixed x = _DirectionX;
                fixed y = _DirectionY;
                fixed ix = i.uv.x;
                fixed iy = i.uv.y;
                
                // 방향 적용
                fixed progress = 
                    x * x * lerp(ix, 1.0 - ix, (x + 1.0) / 2.0) +
                    y * y * lerp(iy, 1.0 - iy, (y + 1.0) / 2.0) - t;

                progress *= 2.0; // 제거되지 않았을 때의 컬러 2배

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 subCol = tex2D(_SubTex, i.uv);

                // sub 텍스처는 원래 텍스처보다 오래, 선명하게 그림
                progress += (progress + 1.0) * subCol.r * 3.0;
                
                // fixedTime이 0일 때 progress가 0이 되게 하기 위해 값을 좀 줄임
                progress -= 1.0;

                // min 0 max 1
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
