// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Test/Unlit/Alpha_2D"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
		_AlphaTex("Base (RGB) Trans. (Alpha)", 2D) = "white" {}
    }

    Category
    {
	Tags{ "Queue"="Transparent+10" "IgnoreProjector"="True" "RenderType"="Transparent"} 
	Blend SrcAlpha OneMinusSrcAlpha 
	Lighting Off 
        ZWrite Off
	ZTest Always
        Cull Off
        SubShader
        {
            Pass
            {

		CGPROGRAM 

		uniform float4 _Color;
		sampler2D _MainTex;
		sampler2D _AlphaTex;

		#pragma vertex vert 
		#pragma fragment frag 

		struct data { 
			float4 vertex : POSITION; 
			float2  uv : TEXCOORD0;
		}; 

		data vert (data v) { 
			v.vertex = UnityObjectToClipPos(v.vertex); 
			return v; 
		} 

		fixed4 frag(data f) : COLOR { 

			half4 col;
			col.rgb = tex2D(_MainTex, f.uv).rgb;
			col.a = tex2D(_AlphaTex, f.uv).r;

			return col*_Color;
		} 

		ENDCG
            }
        } 
    }
}