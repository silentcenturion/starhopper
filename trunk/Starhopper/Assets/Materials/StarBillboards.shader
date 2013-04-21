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
			float _Saturation;

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
			
			v2f vert (appdata v)
			{
				v2f o;

				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);

				float2 screenPixel = (_ScreenParams.zw - 1) * max(3, min(_MaxSize, (1 - (-(mul( UNITY_MATRIX_MV, v.vertex ).z / _PixelSizeDistance))) * _MaxSize));
				o.pos = v.vertex;
				o.pos = mul (UNITY_MATRIX_MVP, o.pos);
				float4 screenTexcoord = mul(UNITY_MATRIX_P, v.texcoord * o.projPos.z * float4(screenPixel.xx, 1, 1));// * _ScreenParams.zw);
				o.pos.xy += screenTexcoord.xy;

			    float4 projPos = ComputeScreenPos(o.pos);
				projPos.xy += screenTexcoord;
				
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
				float3 normalizedColor = normalize(c.rgb);
				float mag = length(c.rgb);
				c.rgb = lerp(normalize(pow(normalizedColor, 15 * _Saturation)) * mag, c.rgb, saturate(1 - (z) - 0.5));
				
				return c; 
			}
			ENDCG
		}
	} 
}