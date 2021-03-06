﻿Shader "Custom/FogOfWarHard" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_DinTex ("Base (RGB)", 2D) = "white" {}
		_StatTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "LightMode"="ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha:blend
		

		fixed4 _Color;
		sampler2D _DinTex;
		sampler2D _StatTex;


		struct Input {
			float2 uv_DinTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{	
			half4 dinColor =  tex2D (_DinTex, IN.uv_DinTex);
	
			half4 statColor =  tex2D (_StatTex, IN.uv_DinTex);

			o.Alpha = 1.8 - (dinColor.g + statColor.g); //green - color of aperture mask
		}
		ENDCG
	} 
	
	Fallback "Transparent"
}
