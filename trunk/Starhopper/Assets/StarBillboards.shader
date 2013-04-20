Shader "Star Billboards" {
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

			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.pos += mul(UNITY_MATRIX_P, v.texcoord * 1);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : COLOR0 
			{ 
				float4 c = 1;
				c.a = saturate(1 - dot(i.texcoord, i.texcoord));
				c.rgb *= c.a;
				return c; 
			}
			ENDCG
		}
	} 
}