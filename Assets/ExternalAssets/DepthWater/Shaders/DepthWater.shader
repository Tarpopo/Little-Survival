
Shader "XVG/DepthWater"
{
	Properties
	{
	
		[Header(COLOR)]
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		_Color ("Water Color (RGB) Opacity (A)", Color) = (0.5,0.5,0.5,1.0)
		
		
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
	

		
		_RampThreshold ("Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.001,1)) = 0.1

		
		[Header(______________________________________________________________________________)]
		[Header(FOAM)]
		
		_FoamSpread ("Foam Spread", Range(0.01,5)) = 2
		_FoamStrength ("Foam Strength", Range(0.01,1)) = 0.8
		_FoamColor ("Foam Color (RGB) Opacity (A)", Color) = (0.9,0.9,0.9,1.0)
		[NoScaleOffset]
		_FoamTex ("Foam (RGB)", 2D) = "white" {}
		_FoamSmooth ("Foam Smoothness", Range(0,0.5)) = 0.02
		_FoamSpeed ("Foam Speed", Vector) = (2,2,2,2)
		
		[Header(______________________________________________________________________________)]
		[Header(DEPTH)]
		[PowerSlider(5.0)] _DepthAlpha ("Depth Alpha", Range(0.01,10)) = 0.5
		_DepthMinAlpha ("Depth Min Alpha", Range(0,1)) = 0.5

		
		[Header(______________________________________________________________________________)]
		[Header(UV Waves Animation)]
		_UVWaveSpeed("Speed", Float) = 1
		_UVWaveAmplitude("Amplitude", Range(0.001,0.5)) = 0.05
		_UVWaveFrequency("Frequency", Range(0,10)) = 1


		
	[Header(______________________________________________________________________________)]
	[Header(SPECULAR)]
		
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Roughness", Range(0.0,10)) = 0.1
	
	[Header(______________________________________________________________________________)]
	[Header(RIM)]
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,1)) = 0.5
		_RimMax ("Rim Max", Range(0,1)) = 1.0
	[Header(______________________________________________________________________________)]
	[Header(TRANSPARENCY)]
		//Blending
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendTCP2 ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendTCP2 ("Blending Dest", Float) = 10
	[Separator]
		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend [_SrcBlendTCP2] [_DstBlendTCP2]


		CGPROGRAM

		#pragma surface surf ToonyColorsWater keepalpha vertex:vert nolightmap
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D_float _CameraDepthTexture;
		half4 _FoamSpeed;
		half _FoamSpread;
		half _FoamStrength;
		sampler2D _FoamTex;
		fixed4 _FoamColor;
		half _FoamSmooth;
		half _DepthAlpha;
		fixed _DepthMinAlpha;
		half unityTime;
		half _UVWaveAmplitude;
		half _UVWaveFrequency;
		half _UVWaveSpeed;

		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;

		

		struct Input
		{
			float2 texcoord;
			half2 bump_texcoord;
			half3 viewDir;
			float3 wPos;
			float2 sinAnim;
			INTERNAL_DATA
			float4 sPos;
			
		};

	
		half4 _HColor;
		half4 _SColor;
		half _RampThreshold;
		half _RampSmooth;
		fixed _Shininess;
		UNITY_INSTANCING_BUFFER_START(Props)
		
		UNITY_INSTANCING_BUFFER_END(Props)

		struct SurfaceOutputWater
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			
		};

		inline half4 LightingToonyColorsWater (inout SurfaceOutputWater s, half3 viewDir, UnityGI gi)
		{
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			s.Normal = normalize(s.Normal);			
			fixed ndl = max(0, dot(s.Normal, lightDir));
			#define NDL ndl
			#define		RAMP_THRESHOLD	_RampThreshold
			#define		RAMP_SMOOTH		_RampSmooth

			fixed3 ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, NDL);
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
		#if !defined(UNITY_PASS_FORWARDBASE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
			c.a = s.Alpha;
			//Specular
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (s.Normal, h));
			float spec = pow(ndh, (s.Specular+1e-4f)*128.0) * s.Gloss * 2.0;
			spec *= atten;
			c.rgb += lightColor.rgb * _SpecColor.rgb * spec;

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif
			return c;
		}

		void LightingToonyColorsWater_GI(inout SurfaceOutputWater s, UnityGIInput data, inout UnityGI gi)
		{
			gi = UnityGlobalIllumination(data, 1.0, s.Normal);

			gi.light.color = _LightColor0.rgb;	//remove attenuation
			s.atten = data.atten;	//transfer attenuation to lighting function
		}




		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 tangent : TANGENT;
	#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
	#endif
		};

			#define TIME (_Time.y)

		void vert(inout appdata_tcp2 v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//Main texture UVs
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float2 mainTexcoords = worldPos.xz * 0.1;
			o.texcoord.xy = TRANSFORM_TEX(mainTexcoords.xy, _MainTex);
			



			// UV Wave animations
			half2 x = ((v.vertex.xy + v.vertex.yz) * _UVWaveFrequency) + (TIME.xx * _UVWaveSpeed);
			o.sinAnim = x;
			
			float4 pos = UnityObjectToClipPos(v.vertex);
			o.sPos = ComputeScreenPos(pos);
			COMPUTE_EYEDEPTH(o.sPos.z);
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputWater o)
		{
			half2 uvDistort = ((sin(0.9 * IN.sinAnim.xy) + sin(1.33 * IN.sinAnim.xy + 3.14) + sin(2.4 * IN.sinAnim.xy + 5.3)) / 3) * _UVWaveAmplitude;
			IN.texcoord.xy += uvDistort.xy;
			half ndv = dot(IN.viewDir, o.Normal);
			fixed4 mainTex = tex2D(_MainTex, IN.texcoord.xy);
			float sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.sPos));
			if(unity_OrthoParams.w > 0)
			{
				//orthographic camera
			#if defined(UNITY_REVERSED_Z)
				sceneZ = 1.0f - sceneZ;
			#endif
				sceneZ = (sceneZ * _ProjectionParams.z) + _ProjectionParams.y;
			}
			else
				//perspective camera
				sceneZ = LinearEyeDepth(sceneZ);
			float partZ = IN.sPos.z;
			float depthDiff = abs(sceneZ - partZ);
			
			//Depth-based foam
			half2 foamUV = IN.texcoord.xy;
			foamUV.xy += TIME.xx*_FoamSpeed.xy*0.05;
			fixed4 foam = tex2D(_FoamTex, foamUV);
			foamUV.xy += TIME.xx*_FoamSpeed.zw*0.05;
			fixed4 foam2 = tex2D(_FoamTex, foamUV);
			foam = (foam + foam2) / 2;
			float foamDepth = saturate(_FoamSpread * depthDiff);
			half foamTerm = (smoothstep(foam.r - _FoamSmooth, foam.r + _FoamSmooth, saturate(_FoamStrength - foamDepth)) * saturate(1 - foamDepth)) * _FoamColor.a;
			o.Albedo = lerp(mainTex.rgb * _Color.rgb, _FoamColor.rgb, foamTerm);
			_Color.a *= saturate((_DepthAlpha * depthDiff) + _DepthMinAlpha);
			o.Alpha = mainTex.a * _Color.a;
			o.Alpha = lerp(o.Alpha, _FoamColor.a, foamTerm);
			
			
			//Specular
			o.Gloss = 1;
			o.Specular = _Shininess;
			//Rim
			half3 rim = smoothstep(_RimMax, _RimMin, 1-Pow4(1-ndv)) * _RimColor.rgb * _RimColor.a;
			o.Emission += rim.rgb;
			half3 eyeVec = IN.wPos.xyz - _WorldSpaceCameraPos.xyz;
			fixed3 reflColor = fixed3(0,0,0);
		

		
			
		}

		ENDCG

	}

	
}
