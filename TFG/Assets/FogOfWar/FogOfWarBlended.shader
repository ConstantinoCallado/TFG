Shader "Custom/FogOfWarBlended" {
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
			const float blurPower = 0.005;
		
			half4 dinColor =  tex2D (_DinTex, IN.uv_DinTex);

			half4 statColor1 = tex2D(_StatTex, IN.uv_DinTex + float2(-blurPower, 0));			
			half4 statColor2 = tex2D(_StatTex, IN.uv_DinTex + float2(blurPower, 0));	
			half4 statColor3 = tex2D(_StatTex, IN.uv_DinTex + float2(0, -blurPower));			
			half4 statColor4 = tex2D(_StatTex, IN.uv_DinTex + float2(0, blurPower));
			half4 statColor = 0.25 * (statColor1 + statColor2 + statColor3 + statColor4);
		
			o.Alpha = (1 - dinColor.g) - statColor.g/5; //green - color of aperture mask
		}
		ENDCG
	} 
	
	Fallback "Transparent"
}
