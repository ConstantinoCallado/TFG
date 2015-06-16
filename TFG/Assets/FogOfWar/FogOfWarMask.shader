Shader "Custom/FogOfWarMask" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_DinTex ("Base (RGB)", 2D) = "white" {}
		_StatTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "LightMode"="ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
		
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
			const float _BlurPower = 0.006;
			
			half4 dinColor =  tex2D (_DinTex, IN.uv_DinTex);

			half4 statColor1 = tex2D(_StatTex, IN.uv_StatTex + float2(-_BlurPower, 0));			
			half4 statColor2 = tex2D(_StatTex, IN.uv_StatTex + float2(_BlurPower, 0));	
			half4 statColor3 = tex2D(_StatTex, IN.uv_StatTex + float2(0, -_BlurPower));			
			half4 statColor4 = tex2D(_StatTex, IN.uv_StatTex + float2(0, _BlurPower));
			half4 statColor = 0.25 * (statColor1 + statColor2 + statColor3 + statColor4);
						
			//half4 statColor =  tex2D (_StatTex, IN.uv_StatTex);
			
			o.Albedo = _Color.rgb;
			o.Alpha = (_Color.a - dinColor.g) - statColor.g/5; //green - color of aperture mask
		}
		ENDCG
	} 
	
	Fallback "Diffuse"
}
