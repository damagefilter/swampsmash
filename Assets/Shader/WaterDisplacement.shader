Shader "Custom/WaterDisplacement" {
	Properties {
		[PerRendererData] _MainTex ("SpriteSheet", 2D) = "white" {}
		_NoiseTex ("Wave Noise Texture", 2D) = "white" {}
		// Add sine wave elngth and stuff?
//		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
    SubShader {
    	Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
        Pass 
        {
    	CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            
            struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

            struct FragmentInput {
            	float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            FragmentInput vert(appdata_t i) 
            {
                FragmentInput o;
                o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
                o.texcoord = i.texcoord;
                o.color = i.color;
                return o;
            }

            fixed4 frag(FragmentInput i) : COLOR 
            {
				float2 displacedTexCoord = i.texcoord + float2(
					tex2D(_NoiseTex, i.vertex.xy/500 + float2((_Time.w%50)/25, 25)).z - .5,                            
					tex2D(_NoiseTex, i.vertex.xy/500 + float2(0, (_Time.w%50)/25)).z - .5
				)/100;
				fixed4 result = tex2D(_MainTex, displacedTexCoord) * i.color;
				return result;
			}
			ENDCG
		}
	}
}