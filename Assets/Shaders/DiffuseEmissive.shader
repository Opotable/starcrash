Shader "Custom/DiffuseEmissive"
{
	Properties
	{
		_Color("Color (RGB)", COLOR) = (1,1,1,1)
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		half4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _Color.rgb * 0.5;
			o.Emission = _Color.rgb * 0.5;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
