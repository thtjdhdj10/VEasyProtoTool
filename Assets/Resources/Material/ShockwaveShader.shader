Shader "Custom/ShockwaveShader"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        [Normal]_NormalMap("NormalMap", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        //Tags { "RenderType"="Opaque" }
        //Tags { "RenderType"="Background" }
        //Tags { "RenderType"="Overlay" }

        Cull Off
        ZWrite Off

        LOD 100

        GrabPass
        {
            "_GrabTexture"
        }

        Pass{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _NormalMap;
            sampler2D _MainTex;
            sampler2D _GrabTexture;

            float4 _MainTex_ST;
            float4 _NormalMap_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 distortionUV : TEXCOORD1;
                float4 grabPassUV : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.distortionUV = TRANSFORM_TEX(v.uv, _NormalMap);
                o.grabPassUV = ComputeGrabScreenPos(o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float amount = fmod(_Time, 1.0);

                //fixed mask = tex2D(_MainTex, i.uv).x;
                float2 distortion = UnpackNormal(tex2D(_NormalMap, i.distortionUV)).xy;
                distortion *= amount;
                i.grabPassUV.xy += distortion * i.grabPassUV.z;
                fixed4 col = tex2Dproj(_GrabTexture, i.grabPassUV);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
