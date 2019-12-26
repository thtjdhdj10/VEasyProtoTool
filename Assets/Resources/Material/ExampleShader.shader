Shader "Custom/ExampleShader"
{
    // https://gat-designer.tistory.com/8
    Properties
    {
        // Inspector 에서 컨트롤 가능한 속성 변수
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Tex (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        //Tags{ "RenderType" = "Transparent" }
        //Tags { "RenderType"="Background" }
        //Tags { "RenderType"="Overlay" }

        //LOD 200
        //Pass{
            CGPROGRAM
            #pragma surface surf Lambert alpha
            //#pragma vertex vert
            //#pragma fragment frag

            //#include "UnityCG.cginc" //include UnityObjectToClipPos

            // 쉐이딩할 변수 재정의
            // half4 _Color;
            sampler2D _MainTex;
            sampler2D _GradientTex;

            struct Input {
                float2 uv_MainTex;
            };

            //// vert에서 받을 파라미터들
            //struct appdata
            //{
            //    float2 uv : TEXCOORD0;
            //    float4 vertex : POSITION;
            //};

            //// frag에서 받을 파라미터들
            //struct v2f
            //{
            //    float2 uv : TEXCOORD0;
            //    float4 vertex : SV_POSITION;
            //};

            //// vertex shader
            //v2f vert(appdata v)
            //{
            //    v2f o;
            //    o.vertex = v.vertex;
            //    o.uv = v.uv;
            //    return o;
            //}

            //// fragment shader
            //fixed4 frag(v2f i) : SV_Target
            //{
            //    fixed4 o;
            //    return o;
            //}
            //fixed4 frag(v2f i) : SV_Target
            //{
            //    // _Time (t/20, t, t*2, t*3)
            //    float fFrame = fmod(_Time.w, 5.0); // 시간에 따른 프레임 계산
            //    // int iFrame = floor(fFrame); // 현재 재생될 프레임 index
            //    // int iNextFrame = floor(fmod(fFrame + 1.0, frameCount));
            //    // 재생 시작 시점의 시간 가져와서
            //    // _Time - 재생시작시간 으로 계산
            //    c.rgb *= c.a;
            //    return c;
            //}

            // surface shader
            void surf(Input IN, inout SurfaceOutput o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Emission = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        //}
    }
    FallBack "Diffuse"
}
