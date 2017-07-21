Shader "Custom/MatCap Hologram" {
	Properties{
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_MainTex2("Base (RGB) Trans (A)", 2D) = "white" {}
		_StripesColor("Color", Color) = (0.5,0.5,0.5,1)
		_Stripes("Base (RGB) Trans (A)", 2D) = "white" {}

		_ScrollX("Scroll X", range(-10, 10)) = 0
		_ScrollY("Scroll Y", range(-10, 10)) = 0

		_VertexPower("Vertex power", range(0,1)) = 1
		_VertexColorMul("Vertex color multiply", float) = 4
	}

	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoord2 : TEXCOORD1;
				float4 color : COLOR;
				half2 texcoord3 : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainTex2;
			float4 _MainTex2_ST;
			float4 _Color;
			float _VertexPower;
			float _VertexColorMul;

			float4 _StripesColor;
			sampler2D _Stripes;
			
			float _ScrollX;
			float _ScrollY;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord2, _MainTex2);
				o.color = v.color;

				float3 worldNorm = normalize(_World2Object[0].xyz * v.normal.x + _World2Object[1].xyz * v.normal.y + _World2Object[2].xyz * v.normal.z);
				worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
				o.texcoord2 = worldNorm.xy * 0.5 + 0.5;

				o.texcoord3 = mul(_Object2World, v.vertex).xy;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 main = tex2D(_MainTex, i.texcoord);
				fixed4 col2 = tex2D(_MainTex2, i.texcoord2);
				fixed4 colmain = main * col2 * _Color * (lerp(fixed4(1, 1, 1, 1), i.color, _VertexPower) * _VertexColorMul);
				fixed2 tc = i.texcoord2.xy;
				tc += float2(_ScrollX * _Time.x, _ScrollY * _Time.x);
				/*tc.x /= i.texcoord.x;
				tc.y /= i.texcoord.y;*/
				fixed4 holo = tex2D(_Stripes, tc) * _StripesColor * 2;
				fixed4 col = lerp(colmain, colmain + holo, colmain.a * _StripesColor.a * (sin(_Time.w) * 0.1));

				return col;
			}
			ENDCG
		}
	}
}
