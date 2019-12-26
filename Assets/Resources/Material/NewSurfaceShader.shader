Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _NormalTex ("_NormalTex", 2D) = "white" {}
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
 
            sampler2D _MainTex;
            sampler2D _NormalTex;
            sampler2D _GrabTexture;
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
                fixed3 centerUV = fixed3(i.uv.x - 0.5, i.uv.y - 0.5, 0.0);
                fixed len = length(centerUV);
                fixed degree = 1.0 - fmod(_Time.y, 2.0) / 2.0;
                
                fixed2 uv2 = i.uv;
                uv2.x *= degree;
                uv2.y *= degree;
                uv2.x += degree;
                uv2.y += degree;
                fixed4 normal = tex2D(_NormalTex, uv2);
                
                fixed2 pos = i.grabPassUV;
                //pos.x += (normal.r * 2.0 - 1.0) * 0.05 * pow(abs(degree - len), 2.0);
                pos.y += (normal.g * 2.0 - 1.0) * 0.05;

                fixed4 col = tex2D(_GrabTexture, pos);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
