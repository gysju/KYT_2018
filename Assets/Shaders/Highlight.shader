﻿Shader "Custom/Highlight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,5)) = 0.0

		[Toggle] _Highlight ("Highlight", Float) = 0
		_HighlightIntensity("Highlight intensity", Range(1,5)) = 1.0
		_HighlightSpeed("Highlight speed", Range(1,5)) = 1.0
	}

	SubShader 
		{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness, _Metallic, _Highlight, _HighlightIntensity, _HighlightSpeed;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = _Highlight > 0 ? c.rgb * lerp(1.0f, _HighlightIntensity, (cos(_Time.y * _HighlightSpeed) * 0.5f + 0.5f)) : c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}