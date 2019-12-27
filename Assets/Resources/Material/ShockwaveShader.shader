Shader "Custom/ShockwaveShader"
{
    Properties
    {
        _ScaleTex ("_ScaleTex", 2D) = "white" {}
        _NormalTex ("_NormalTex", 2D) = "white" {}
        _Duration ("_Duration", Float) = 0.75
        _Power ("_Power", Float) = 1.0
        _GeneratedTime ("_GeneratedTime", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Cull Off ZWrite Off ZTest Always
        LOD 100

        // Grab the screen behind the object into _GrabTexture
        GrabPass { 
            "_GrabTexture"
        }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            sampler2D _ScaleTex;
            sampler2D _NormalTex;
            sampler2D _GrabTexture;
            float _GeneratedTime;
            float _Duration;
            float _Power;
            float4 _MainTex_ST;
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 grabPassUV : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPassUV = ComputeGrabScreenPos(o.vertex);

                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed fixedTime = fmod(_Time.y - _GeneratedTime, _Duration) / _Duration; // 0~1

                fixed scale = fixedTime; // 0~1
                scale = 1 - scale;
                scale *= scale;
                scale = 1 - scale; // 빠르게 증가하다 감속하는 그래프

                float power = 1.0 - fixedTime; // 1~0
                // 제곱해서 그래프 모양 exponential하게 하고, power 계수 곱하고, _Power 곱하고
                power *= power * 0.025 * _Power;

                fixed formedScale = lerp(5.0, 1.0, scale);

                // 스케일링하고 중앙으로 이동
                // 값이 크면 uv max 1을 초과하여 무시됨
                fixed2 scaledUV = i.uv;
                scaledUV *= formedScale;
                scaledUV -= formedScale / 2.0 - 0.5;

                fixed4 normal = tex2D(_NormalTex, scaledUV);
                
                fixed2 pos = i.grabPassUV;
                pos.x += (normal.r * 2.0 - 2.0) * power; // TODO 왜 2.0을 빼야 맞지?
                pos.y -= (normal.g * 2.0 - 1.0) * power;

                return tex2D(_GrabTexture, pos);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
