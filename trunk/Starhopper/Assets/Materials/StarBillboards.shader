Shader "StarBillboards" {
Properties {
	_Distance ("Distance?", Float) = 1
	_Brightness ("Brightness", Float) = 1
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
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				float2 screenTexcoord = mul(UNITY_MATRIX_P, v.texcoord);// * _ScreenParams.zw);
				o.pos.xy += screenTexcoord;
				o.texcoord = v.texcoord.xy;
				o.color = v.color;
				
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);
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