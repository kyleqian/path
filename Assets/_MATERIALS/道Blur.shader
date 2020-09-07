Shader "Unlit/道Blur"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _UserX ("User X", Float) = 0.0
        _UserY ("User Y", Float) = 0.0
        _BlurDist ("Blur Pixel Distance", Int) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct vdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UserX;
            float _UserY;
            int _BlurDist;

            v2f vert (vdata IN, out float4 vertex : SV_POSITION)
            {
                v2f OUT;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                vertex = UnityObjectToClipPos(IN.vertex);
                return OUT;
            }

            fixed4 frag (v2f IN, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                #if UNITY_UV_STARTS_AT_TOP
                _UserY = _ScreenParams.y - _UserY;
                #endif

                float dist = distance(float2(_UserX, _UserY), screenPos.xy);
                return fixed4(_Color.rgb, 1 - step(_BlurDist, dist));
            }
            ENDCG
        }
    }
}
