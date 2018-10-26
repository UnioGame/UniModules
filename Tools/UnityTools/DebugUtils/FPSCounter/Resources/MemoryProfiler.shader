Shader "Custom/Profile/MemoryProfiler" {
	SubShader { 

	Tags { "Queue" = "Overlay+1000" }

		Pass {

			
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#include "UnityCG.cginc"
		
			float _Texel;
			fixed _Alpha;
			half _Height;

			struct appdata {
				float3 pos : POSITION;
			};

			struct v2f {
				fixed4 color : COLOR0;
				float4 pos: POSITION;
			};

			v2f vert (appdata IN) {
				v2f o;
				o.color = lerp(fixed4(1,0,0,_Alpha), fixed4(0,1,0,_Alpha), IN.pos.y);
				IN.pos.x = -1 + IN.pos.x * _Texel;
				IN.pos.y = 1 - IN.pos.y * _Height;
				o.pos = float4(IN.pos, 1); //UnityObjectToClipPos(IN.pos);
				return o;
			}

			fixed4 frag (v2f IN) : SV_Target {
				return IN.color;
			}

			ENDCG
		}
	}
}