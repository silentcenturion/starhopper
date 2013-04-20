Shader "StarBillboards" {
Properties {
	_Distance ("Distance?", Float) = 1
	_Brightness ("Brightness", Float) = 1
	_PixelSizeDistance ("Pixel Size Distance", Float) = 10000
	_MaxSize ("Max size", Float) = 100
}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Cull Off
		Blend One One
		Fog {Mode Off}
		ZWrite Off
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Distance;
			float _Brightness;
			float _PixelSizeDistance;
			float _MaxSize;

			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
				float4 projPos : TEXCOORD1;
			};

	/*inline float4 ComputeScreenPos (float4 pos) {
		float4 o = pos * 0.5f;
		#if defined(UNITY_HALF_TEXEL_OFFSET)
		o.xy = float2(o.x, o.y*_ProjectionParams.x) + o.w * _ScreenParams.zw;
		#else
		o.xy = float2(o.x, o.y*_ProjectionParams.x) + o.w;
		#endif
	
		#if defined(SHADER_API_FLASH)
		o.xy *= unity_NPOTScale.xy;
		#endif
	
		o.zw = pos.zw;
		return o;
	}*/


			float4 InvertScreenPos(float4 screenPos)
			{
				#if defined(SHADER_API_FLASH)
				screenPos.xy /= unity_NPOTScale.xy;
				#endif

				#if defined(UNITY_HALF_TEXEL_OFFSET)
				screenPos.xy -= screenPos.w * 0.5 / _ScreenParams.zw;
				screenPos.xy = float2(screenPos.x, screenPos.y / _ProjectionParams.x);
				#else
				screenPos.xy -= screenPos.w * 0.5;
				screenPos.xy = float2(screenPos.x, screenPos.y / _ProjectionParams.x);
				#endif

				screenPos.xy *= 2;
				return screenPos;
			}

			v2f vert (appdata v)
			{
				v2f o;

				//o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, v.vertex) + float4(v.texcoord.x, v.texcoord.y, 0.0, 0.0));

				
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);

				float2 screenPixel = (_ScreenParams.zw - 1) * max(3, min(_MaxSize, (1 - (-(mul( UNITY_MATRIX_MV, v.vertex ).z / _PixelSizeDistance))) * _MaxSize));
				o.pos = v.vertex;
				o.pos = mul (UNITY_MATRIX_MVP, o.pos);
				float4 screenTexcoord = mul(UNITY_MATRIX_P, v.texcoord * o.projPos.z * float4(screenPixel.xx, 1, 1));// * _ScreenParams.zw);
				o.pos.xy += screenTexcoord.xy;

			    float4 projPos = ComputeScreenPos(o.pos);
				projPos.xy += screenTexcoord;
			    //o.pos = InvertScreenPos(projPos);

				//float2 screenTexcoord = float2(v.texcoord.x, v.texcoord.y*_ProjectionParams.x) + o.pos.w * _ScreenParams.zw;
				//o.pos.xy += screenTexcoord.xy;

				o.texcoord = v.texcoord.xy;
				o.color = v.color;
				
				return o;
			}

			fixed4 frag (v2f i) : COLOR0 
			{ 
				float4 c = i.color;
				c.a = saturate(1 - dot(i.texcoord, i.texcoord));
				float z = saturate(LinearEyeDepth (i.projPos.z) * _Distance);


				// Color based on distance
				//c.rgb *= lerp(float3(1,0,0), float3(0,1,0), z);
				c.rgb *= _Brightness * (1 + i.color.a);
				c.rgb *= c.a * z;
				return c; 
			}
			ENDCG
		}
	} 
}