Shader "LuckyHammers/UI/ColorTweaks"
{
	// Possible approaches:
	// http://www.graficaobscura.com/matrix/
	// http://forum.unity3d.com/threads/hue-saturation-brightness-contrast-shader.260649/
	// https://github.com/greggman/hsva-unity
	// http://gamedev.stackexchange.com/questions/28782/hue-saturation-brightness-contrast-effect-in-hlsl
	// https://gist.github.com/anonymous/70cfd7891ab46ab19224
	// http://forum.unity3d.com/threads/how-to-make-shader-working-right-in-ugui-mask.284837/

	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Saturation("Saturation", Range(0,2)) = 0
		_Brightness("Brightness", Range(0,4)) = 1
		_Add("Add", Range(0,1)) = 0
		_AddColor("AddColor", Color) = (1.0, 1.0, 1.0, 1.0)

		// TODO: Keep graphic color/alpha to apply in pixels ( Use Grayscale.shader solution when working )
		//_Color("Color", Color) = (1, 1, 1, 1) // color
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _Color;
			fixed _Saturation;
			fixed _Brightness;
			fixed _Add;
			half4 _AddColor;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				half2 texcoord  : TEXCOORD0;
			};

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 c = tex2D(_MainTex, IN.texcoord);

				c.rgb *= _Color.rgb * _Brightness;

				if (_Saturation != 1)
				{
					fixed grayscale = Luminance(c.rgb);
					c.rgb = lerp(grayscale.xxx, c.rgb, _Saturation);
				}
				if (_Add > 0) c.rgb += _AddColor.rgb * _Add.xxx;

				// Set _Color to control alpha completely, when not transparent
				c.a = c.a*_Color.a;

				return c;
			}
			ENDCG
		}
	}
}