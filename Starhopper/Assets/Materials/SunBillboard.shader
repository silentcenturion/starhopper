Shader "SunBillboard" {
Properties {
	_NoiseCube ("Noise Cube", Cube) = "white" {}
	_CloudCube ("Cloud Cube", Cube) = "white" {}
	_Brightness ("Brightness", Float) = 1
	_Size ("Size", Range(0, 3)) = 1
}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		Fog {Mode Off}
		ZWrite Off
		ColorMask RGB
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			float _Distance;
			float _Brightness;
			float _PixelSizeDistance;
			float _MaxSize;
			float _Size;
			samplerCUBE _NoiseCube;
			samplerCUBE _CloudCube;
			float4x4 _Rotation;

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
			
				float2 size = (v.texcoord.xy * 0.1 * _Size) / unity_Scale.w;
			    o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(size, 0.0, 0.0));
								
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);
				 
				o.texcoord = v.texcoord.xy; 
				o.color = v.color;
				
				return o;
			}

			fixed4 frag (v2f i) : COLOR0 
			{ 
				float4 c = i.color;
				
				c.a = saturate(1 - dot(i.texcoord, i.texcoord));
				float3 sphereDirection = float3(i.texcoord.xy, sqrt(1 - i.texcoord.x * i.texcoord.x - i.texcoord.y * i.texcoord.y));
				sphereDirection = mul((float3x3)UNITY_MATRIX_T_MV, sphereDirection);
				
				float3 sphereDirection2 = mul((float3x3)_Rotation, sphereDirection);
				
				float3 baseColor = pow(i.color.rgb, 7);
				float3 noise = texCUBE(_NoiseCube, sphereDirection).rgb;
				float3 cloud = texCUBE(_CloudCube, sphereDirection).rgb;

				c.rgb *= c.a;
				float noiseAndClouds = (noise.r * noise.g + cloud.r);
				float3 noise2 = texCUBE(_NoiseCube, sphereDirection2).rgb;
				float3 cloud2 = texCUBE(_CloudCube, sphereDirection2).rgb;
				float noiseAndClouds2 = (noise2.r * noise2.g + cloud2.r);
				
				c.rgb = (0.4 + noiseAndClouds) * noiseAndClouds2 * baseColor * _Brightness;
				c.rgb = pow(c.rgb, 5);
				
				c.a = saturate(saturate(1 - dot(i.texcoord, i.texcoord)) * 50);
				return c; 
			}
			ENDCG
		}
	} 
}