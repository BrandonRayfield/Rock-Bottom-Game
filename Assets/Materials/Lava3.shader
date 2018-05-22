﻿Shader "Custom/Lava3"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//o.vertex.y += sin(worldPos.x + _Time.w);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, IN.uv + float2(0, sin( IN.vertex.x/1000 + _Time[1] )/10));
				// apply fog
				UNITY_APPLY_FOG(IN.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}