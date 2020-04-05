Shader "Custom/AntHole"
{
    Properties
    {
        _Speed ("Speed", Range(0, 10)) = 0
        _Alpha ("Alpha", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Offset -1 , -1
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            uniform float _Speed;
            uniform float _Alpha;

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

            fixed4 circle(float2 st)
            {
                float dis = distance(0.5, st);
                clip(0.3 - dis);
                float col = step(0.3, dis);
                return fixed4(col, col, col, _Alpha);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = 2 * i.uv.y + sin(_Time.y);
                float distort = sin(_Time.y * _Speed) * 0.1 * sin(1.5f * x) * (-(x - 1) * (x - 1) + 1);
                i.uv.x += distort;
                return circle(i.uv);
            }
            ENDCG
        }
    }
}
