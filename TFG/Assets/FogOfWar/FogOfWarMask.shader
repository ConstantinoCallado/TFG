﻿Shader "Custom/FogOfWarMask" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_DinTex ("Base (RGB)", 2D) = "white" {}
		_StatTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"RenderType"="Transparent" "LightMode"="ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha:blend
		
		
		//fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten)
		//{
		//	fixed4 color;
		//	color.rgb = s.Albedo;
		//	color.a = s.Alpha;
		//	return color;
		//}

		fixed4 _Color;
		sampler2D _DinTex;
		sampler2D _StatTex;

		struct Input {
			float2 uv_DinTex;
			float2 uv_StatTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{				
			half4 dinColor =  tex2D (_DinTex, IN.uv_DinTex);
			half4 statColor =  tex2D (_StatTex, IN.uv_StatTex);
			
			o.Albedo = _Color.rgb;
			o.Alpha = (_Color.a - dinColor.g) - statColor.g/5; //green - color of aperture mask
		}
		ENDCG
	} 
	
	Fallback "Diffuse"
}
