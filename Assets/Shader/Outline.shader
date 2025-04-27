Shader "Custom/OutlineOnly"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0, 0.5, 1, 1)
        _OutlineWidth("Outline Width", Float) = 0.02
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Front    // Important: Render backside for inverted hull
        Lighting Off
        ZWrite On
        ColorMask RGB
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
