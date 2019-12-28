Shader "Custom/Body" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SubTex("Sub Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		[HDR] _EmissionColor("EmissionColor", Color) = (0.000000,0.000000,0.000000,1.000000)
		_EmissionMap("Emission", 2D) = "white" { }
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }

			Pass {
				Name "FORWARD"
				Tags { "LightMode" = "ForwardBase" }
				CGPROGRAM
			// compile directives
		   #pragma vertex vert_surf
		   #pragma fragment frag_surf
		   #pragma target 3.0
		   #pragma multi_compile_fwdbase

		   #include "HLSLSupport.cginc"
		   #include "UnityShaderVariables.cginc"
		   #include "UnityShaderUtilities.cginc"
		   #include "UnityCG.cginc"
		   #include "Lighting.cginc"
		   #include "UnityPBSLighting.cginc"
		   #include "AutoLight.cginc"

			sampler2D _MainTex; float4 _MainTex_ST;
			uniform sampler2D _SubTex;
			uniform sampler2D _MaskTex;
			uniform sampler2D _EmissionMap;
			float4 _EmissionColor;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			struct Input {
				float2 uv_MainTex;
			};

			struct v2f_surf {
				UNITY_POSITION(pos);
				float2 pack0 : TEXCOORD0; // _MainTex
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_SHADOW_COORDS(5)
			};

			// vertex shader
			v2f_surf vert_surf(appdata_full v) {
				v2f_surf o;
				UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				UNITY_TRANSFER_LIGHTING(o,v.texcoord1.xy); // pass shadow and, possibly, light cookie coordinates to pixel shader
				return o;
			}

			//surface shader
			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) ;

				//from blend
				fixed4 c1 = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 c2 = tex2D(_SubTex, IN.uv_MainTex)* _Color;
				fixed4 p = tex2D(_MaskTex, IN.uv_MainTex);

				o.Albedo = c.rgb*lerp(c1, c2, p) + tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}

			// fragment shader
			fixed4 frag_surf(v2f_surf IN) : SV_Target {
				//surf
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT(SurfaceOutputStandard, o);
				Input input;
				input.uv_MainTex = IN.pack0;
				o.Emission = 0.0;
				o.Occlusion = 1.0;
				o.Normal = IN.worldNormal;

				surf(input,o);

				float3 worldPos = IN.worldPos;
			   #ifndef USING_DIRECTIONAL_LIGHT
				  fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
			   #else
				  fixed3 lightDir = _WorldSpaceLightPos0.xyz;
			   #endif
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
				fixed4 c = 0;

				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.light.color = _LightColor0.rgb;
				gi.light.dir = lightDir;

				UnityGIInput giInput;
				UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
				giInput.light = gi.light;
				giInput.worldPos = worldPos;
				giInput.worldViewDir = worldViewDir;
				giInput.atten = atten;

				giInput.probeHDR[0] = unity_SpecCube0_HDR;
				giInput.probeHDR[1] = unity_SpecCube1_HDR;
			   #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
					giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
			   #endif
				LightingStandard_GI(o, giInput, gi);
				c += LightingStandard(o, worldViewDir, gi);
				return c;
			}
		ENDCG
		}
		}
			FallBack "Diffuse"
}