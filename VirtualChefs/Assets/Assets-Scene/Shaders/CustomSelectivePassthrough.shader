// Copyright (c) Meta Platforms, Inc. and affiliates.

Shader "MixedReality/CustomSelectivePassthrough"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_InvertedAlpha("Inverted Alpha", float) = 1

        [Header(DepthTest)]
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 0 //"Off"
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpColor("Blend Color", Float) = 0 //"Add"
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOpAlpha("Blend Alpha", Float) = 2 //"ReverseSubstract"
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue"="Transparent-1"}
        LOD 100

        Pass
        {
		    Cull[_Cull]
            ZWrite[_ZWrite]
            ZTest[_ZTest]
            BlendOp[_BlendOpColor],[_BlendOpAlpha]
            Blend Zero One, One One

            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members center)
//#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _InvertedAlpha;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                fixed4 col = tex2D(_MainTex, i.uv);
                float alpha = lerp(col.r, 1 - col.r, _InvertedAlpha);
                return float4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
