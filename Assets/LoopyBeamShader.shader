Shader "Unlit/LoopyBeamShader"
{
    Properties
    {
        _MainTex ("pointsTexture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 worldPos2D;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos2D = v.vertex;//UnityObjectToWorldPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                for(int i = 0; i < _MainTex.height; i++){
                    half2 n1;
                    half2 n2;
                    half2 n3;
                    half2 n4;
                    fixed4 col;
                    
                    half2 uv = half2(0, i); 
                    col = SAMPLE_TEXTURE_2D(_MainTex, uv);
                    n1 = half2(col.xy);

                    half2 uv = half2(1, i); 
                    col = SAMPLE_TEXTURE_2D(_MainTex, uv);
                    n2 = half2(col.xy);

                    half2 uv = half2(2, i); 
                    col = SAMPLE_TEXTURE_2D(_MainTex, uv);
                    n3 = half2(col.xy);
                    
                    half2 uv = half2(3, i); 
                    col = SAMPLE_TEXTURE_2D(_MainTex, uv);
                    n4 = half2(col.xy);

                    half dot1 = dot(n1, n2);
                    half dot2 = dot(n2, n3);
                    half dot3 = dot(n3, n4);
                    half dot4 = dot(n4, n1);

                    }
                }

                return col;
            }
            ENDHLSL
        }
    }
}
