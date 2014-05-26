Shader "Custom/Particles"
{
//	Properties
//	{
//		_Color("Color (RGB)", COLOR) = (1,1,1,1)
//		_TintColor("Color (RGB)", COLOR) = (1,1,1,1)
//	}
	
	SubShader
	{
		Tags{"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		struct Input
		{
			float4 color:COLOR;
		};
		
//		fixed4 _Color;
//		fixed4 _TintColor;

		void surf(Input IN, inout SurfaceOutput o)
		{
//			o.Albedo = _Color.rgb * 0.5;
//			o.Emission = _TintColor.rgb * 0.5;
//			o.Emission = half3(0.1, 0.2, 0.3);
			
			o.Albedo = IN.color.rgb * 0.5;
			o.Emission = IN.color.rgb * 0.5;
			o.Alpha = IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
