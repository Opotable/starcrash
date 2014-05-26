Shader "Custom/Bloop"
{
	Properties
	{
		_Color("Color (RGB)", COLOR) = (1,1,1,1)
		_Bloop("Bloop", Range(0, 1)) = 0
	}
	
	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		struct Input
		{
			fixed n;
		};
		
		half _Bloop;
		half4 _Color;
		
		void vert(inout appdata_full v)
		{
			half xy = 1 + (sin(_Time.y * 10) + 1) * 0.5 * _Bloop;
			half z = 1 + (cos(_Time.y * 10) + 1) * 0.5 * _Bloop;
			half3 bloop = half3(xy, xy, z);
			
			v.vertex.xyz *= bloop;
		}
		
		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _Color.rgb * 0.5;
			o.Emission = _Color.rgb * 0.5;
		}
		ENDCG
	}
	Fallback "Diffuse"
}