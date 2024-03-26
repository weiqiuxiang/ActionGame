Shader "Custom/URPBillboard"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        ZTest Always
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            CBUFFER_END

            v2f vert(appdata v)
            {
                float4 objScale = float4(
                    length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                    length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                    length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z)),
                    0
                );

                v2f o;

                o.vertex = TransformObjectToHClip(
                    TransformWorldToObject(
                        (mul(UNITY_MATRIX_I_V, v.vertex * objScale) + (mul(unity_ObjectToWorld, float4(0, 0, 0, 1)))).xyz
                    )
                );
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
}
