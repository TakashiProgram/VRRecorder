// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Girls/Chara/CH_DR_Skin_V3_DEF"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Toggle(_MATERIAL_CHECK_ON)] _Material_Check("Material_Check", Float) = 0
		_OutLine_Color("++OutLine_Color++++++", Color) = (1,1,1,1)
		_Outline_Color_Blend("Outline_Color_Blend", Range( 0 , 1)) = 0
		_Saturation_Outline("Saturation_Outline", Range( 0 , 5)) = 2.5
		_Blightness_Outline("Blightness_Outline", Range( 0 , 1)) = 0.9
		_Line_width("***Line_width***", Range( 0 , 10)) = 1
		_MainColor_DRFA("++MainColor_DRFA++++++", Color) = (1,1,1,1)
		_BlendMainColor("BlendMainColor", Range( 0 , 1)) = 0
		[Toggle(_RECEIVESHADOW_ON_ON)] _ReceiveShadow_On("++ReceiveShadow_On++++", Float) = 1
		_ToonRamp_Mask("ToonRamp_Mask", 2D) = "white" {}
		_LowColor_S("LowColor_S", Range( 0 , 10)) = 2
		_LowColor_V("LowColor_V", Range( 0 , 1)) = 0.9
		_Shadow_Opacity("Shadow_Opacity", Range( 0 , 1)) = 0.5
		_Tex_Base("Tex_Base", 2D) = "white" {}
		_Tex_Decal("Tex_Decal", 2D) = "black" {}
		_Tex_Shadow("Tex_Shadow", 2D) = "white" {}
		_c_DotTex1("c_DotTex1", 2D) = "white" {}
		_c_Gradation("c_Gradation", 2D) = "white" {}
		[Toggle]_c_reverse("c_reverse", Float) = 1
		_c_Level("c_Level", Range( 0 , 1)) = 0
		_c_Smooth("c_Smooth", Float) = 0.2
		_Bottom("Bottom", Float) = 0
		_Top("Top", Float) = 1.6
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0"}
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 objToWorld39_g359 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float temp_output_58_0_g359 = saturate( ( ( length( ( objToWorld39_g359 - _WorldSpaceCameraPos ) ) - 0.5 ) / ( 17.0 - 0.5 ) ) );
			float temp_output_56_0_g359 = saturate( ( ( abs( ( ( atan( ( 1.0 / float4( UNITY_MATRIX_P[0][1],UNITY_MATRIX_P[1][1],UNITY_MATRIX_P[2][1],UNITY_MATRIX_P[3][1] ).y ) ) * 2.0 ) * ( 180.0 / UNITY_PI ) ) ) - 13.0 ) / ( 165.0 - 13.0 ) ) );
			float outlineVar = ( ( ( ( ( temp_output_58_0_g359 * 0.45 ) + ( ( 1.0 - 0.45 ) * temp_output_56_0_g359 ) ) * ( 0.01 - 0.0003 ) ) + 0.0003 ) * _Line_width );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float2 Standerd_UV648 = i.uv_texcoord;
			float4 tex2DNode487 = tex2D( _Tex_Base, Standerd_UV648 );
			float4 tex2DNode659 = tex2D( _Tex_Decal, Standerd_UV648 );
			float4 lerpResult661 = lerp( tex2DNode487 , tex2DNode659 , tex2DNode659.a);
			float4 lerpResult413 = lerp( lerpResult661 , _MainColor_DRFA , _BlendMainColor);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g350 = Mtex528;
			float3 hsvTorgb198_g350 = RGBToHSV( temp_output_348_0_g350.rgb );
			float3 hsvTorgb219_g350 = HSVToRGB( float3(hsvTorgb198_g350.x,( hsvTorgb198_g350.y * _LowColor_S ),( hsvTorgb198_g350.z * _LowColor_V )) );
			float4 appendResult233_g350 = (float4(hsvTorgb219_g350 , (temp_output_348_0_g350).a));
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_24_0_g351 = ase_lightColor;
			float4 break26_g351 = temp_output_24_0_g351;
			float temp_output_217_29_g350 = ( ( break26_g351.r * 0.1769 ) + ( break26_g351.g * 0.8124 ) + ( break26_g351.b * 0.0107 ) );
			float4 temp_output_24_0_g352 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g352 = temp_output_24_0_g352;
			float temp_output_18_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break28_g352.r * 0.4898 ) + ( break28_g352.g * 0.3101 ) + ( break28_g352.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g352 = temp_output_24_0_g352;
			float temp_output_11_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break26_g352.r * 0.1769 ) + ( break26_g352.g * 0.8124 ) + ( break26_g352.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g352 = temp_output_24_0_g352;
			float temp_output_4_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break12_g352.r * 0.0 ) + ( break12_g352.g * 0.01 ) + ( break12_g352.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g353 = (float4(( ( temp_output_18_0_g353 * 2.3655 ) + ( temp_output_11_0_g353 * -0.8971 ) + ( temp_output_4_0_g353 * -0.4683 ) ) , ( ( temp_output_18_0_g353 * -0.5151 ) + ( temp_output_11_0_g353 * 1.4264 ) + ( temp_output_4_0_g353 * 0.0887 ) ) , ( ( temp_output_18_0_g353 * 0.0052 ) + ( temp_output_11_0_g353 * -0.0144 ) + ( temp_output_4_0_g353 * 1.0089 ) ) , 0.0));
			float3 Wnorldm258 = (WorldNormalVector( i , float3(0,0,1) ));
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult184_g348 = dot( Wnorldm258 , ase_worldlightDir );
			float2 temp_cast_2 = (( ( dotResult184_g348 + 1.0 ) * 0.5 )).xx;
			float ToonRamp354_g348 = tex2D( _ToonRamp_Mask, temp_cast_2 ).r;
			#ifdef _RECEIVESHADOW_ON_ON
				float staticSwitch201_g348 = 1;
			#else
				float staticSwitch201_g348 = 1.0;
			#endif
			float clampResult332_g348 = clamp( ( ( ToonRamp354_g348 * staticSwitch201_g348 ) + 0.0 ) , 0.0 , 1.0 );
			float temp_output_751_385 = ( tex2D( _Tex_Shadow, Standerd_UV648 ).r * clampResult332_g348 );
			float clampResult389_g350 = clamp( ( temp_output_751_385 + ( 1.0 - _Shadow_Opacity ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g350 = lerp( ( appendResult233_g350 * appendResult5_g353 ) , ( ase_lightColor * temp_output_348_0_g350 ) , clampResult389_g350);
			float4 Light_Shadow521 = lerpResult259_g350;
			float3 hsvTorgb92_g359 = RGBToHSV( Light_Shadow521.xyz );
			float3 hsvTorgb91_g359 = HSVToRGB( float3(hsvTorgb92_g359.x,( hsvTorgb92_g359.y * _Saturation_Outline ),( hsvTorgb92_g359.z * _Blightness_Outline )) );
			float4 lerpResult127_g359 = lerp( float4( hsvTorgb91_g359 , 0.0 ) , _OutLine_Color , _Outline_Color_Blend);
			float Mtex_Alpha593 = tex2DNode487.a;
			float temp_output_48_0_g357 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_7 = (( _c_Level - temp_output_48_0_g357 )).xxxx;
			float4 temp_cast_8 = (( _c_Level + temp_output_48_0_g357 )).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g357 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g357 = tex2D( _c_Gradation, appendResult39_g357 );
			float4 smoothstepResult54_g357 = smoothstep( temp_cast_7 , temp_cast_8 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g357 ) ):( tex2DNode41_g357 )) ) ));
			float4 blendOpSrc59_g357 = smoothstepResult54_g357;
			float4 blendOpDest59_g357 = ( 1.0 - smoothstepResult54_g357 );
			float4 temp_output_59_0_g357 = ( saturate( ( blendOpSrc59_g357 * blendOpDest59_g357 ) ));
			float2 temp_output_53_0_g357 = ( appendResult39_g357 * float2( 5,5 ) );
			float2 panner57_g357 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g357);
			float mulTime55_g357 = _Time.y * 0.5;
			float2 panner58_g357 = ( mulTime55_g357 * float2( 0,0 ) + temp_output_53_0_g357);
			float4 blendOpSrc64_g357 = ( temp_output_59_0_g357 + temp_output_59_0_g357 );
			float4 blendOpDest64_g357 = ( tex2D( _c_DotTex1, panner57_g357 ) * tex2D( _c_DotTex1, panner58_g357 ) );
			float4 temp_output_64_0_g357 = ( saturate( ( blendOpSrc64_g357 * blendOpDest64_g357 ) ));
			float4 clampResult69_g357 = clamp( ( smoothstepResult54_g357 + temp_output_64_0_g357 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_773_0 = ( Mtex_Alpha593 * clampResult69_g357 );
			o.Emission = lerpResult127_g359.rgb;
			clip( temp_output_773_0.r - _Cutoff );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _MATERIAL_CHECK_ON
		#pragma shader_feature _RECEIVESHADOW_ON_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Tex_Base;
		uniform float _c_Level;
		uniform float _c_Smooth;
		uniform float _c_reverse;
		uniform sampler2D _c_Gradation;
		uniform float _Bottom;
		uniform float _Top;
		uniform sampler2D _c_DotTex1;
		uniform sampler2D _Tex_Decal;
		uniform float4 _MainColor_DRFA;
		uniform float _BlendMainColor;
		uniform float _LowColor_S;
		uniform float _LowColor_V;
		uniform sampler2D _Tex_Shadow;
		uniform sampler2D _ToonRamp_Mask;
		uniform float _Shadow_Opacity;
		uniform float _Cutoff = 0.5;
		uniform float _Line_width;
		uniform float _Saturation_Outline;
		uniform float _Blightness_Outline;
		uniform float4 _OutLine_Color;
		uniform float _Outline_Color_Blend;


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 Standerd_UV648 = i.uv_texcoord;
			float4 tex2DNode487 = tex2D( _Tex_Base, Standerd_UV648 );
			float Mtex_Alpha593 = tex2DNode487.a;
			float temp_output_48_0_g357 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_0 = (( _c_Level - temp_output_48_0_g357 )).xxxx;
			float4 temp_cast_1 = (( _c_Level + temp_output_48_0_g357 )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g357 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g357 = tex2D( _c_Gradation, appendResult39_g357 );
			float4 smoothstepResult54_g357 = smoothstep( temp_cast_0 , temp_cast_1 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g357 ) ):( tex2DNode41_g357 )) ) ));
			float4 blendOpSrc59_g357 = smoothstepResult54_g357;
			float4 blendOpDest59_g357 = ( 1.0 - smoothstepResult54_g357 );
			float4 temp_output_59_0_g357 = ( saturate( ( blendOpSrc59_g357 * blendOpDest59_g357 ) ));
			float2 temp_output_53_0_g357 = ( appendResult39_g357 * float2( 5,5 ) );
			float2 panner57_g357 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g357);
			float mulTime55_g357 = _Time.y * 0.5;
			float2 panner58_g357 = ( mulTime55_g357 * float2( 0,0 ) + temp_output_53_0_g357);
			float4 blendOpSrc64_g357 = ( temp_output_59_0_g357 + temp_output_59_0_g357 );
			float4 blendOpDest64_g357 = ( tex2D( _c_DotTex1, panner57_g357 ) * tex2D( _c_DotTex1, panner58_g357 ) );
			float4 temp_output_64_0_g357 = ( saturate( ( blendOpSrc64_g357 * blendOpDest64_g357 ) ));
			float4 clampResult69_g357 = clamp( ( smoothstepResult54_g357 + temp_output_64_0_g357 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_773_0 = ( Mtex_Alpha593 * clampResult69_g357 );
			float4 tex2DNode659 = tex2D( _Tex_Decal, Standerd_UV648 );
			float4 lerpResult661 = lerp( tex2DNode487 , tex2DNode659 , tex2DNode659.a);
			float4 lerpResult413 = lerp( lerpResult661 , _MainColor_DRFA , _BlendMainColor);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g350 = Mtex528;
			float3 hsvTorgb198_g350 = RGBToHSV( temp_output_348_0_g350.rgb );
			float3 hsvTorgb219_g350 = HSVToRGB( float3(hsvTorgb198_g350.x,( hsvTorgb198_g350.y * _LowColor_S ),( hsvTorgb198_g350.z * _LowColor_V )) );
			float4 appendResult233_g350 = (float4(hsvTorgb219_g350 , (temp_output_348_0_g350).a));
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_24_0_g351 = ase_lightColor;
			float4 break26_g351 = temp_output_24_0_g351;
			float temp_output_217_29_g350 = ( ( break26_g351.r * 0.1769 ) + ( break26_g351.g * 0.8124 ) + ( break26_g351.b * 0.0107 ) );
			float4 temp_output_24_0_g352 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g352 = temp_output_24_0_g352;
			float temp_output_18_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break28_g352.r * 0.4898 ) + ( break28_g352.g * 0.3101 ) + ( break28_g352.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g352 = temp_output_24_0_g352;
			float temp_output_11_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break26_g352.r * 0.1769 ) + ( break26_g352.g * 0.8124 ) + ( break26_g352.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g352 = temp_output_24_0_g352;
			float temp_output_4_0_g353 = ( temp_output_217_29_g350 * pow( ( ( break12_g352.r * 0.0 ) + ( break12_g352.g * 0.01 ) + ( break12_g352.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g353 = (float4(( ( temp_output_18_0_g353 * 2.3655 ) + ( temp_output_11_0_g353 * -0.8971 ) + ( temp_output_4_0_g353 * -0.4683 ) ) , ( ( temp_output_18_0_g353 * -0.5151 ) + ( temp_output_11_0_g353 * 1.4264 ) + ( temp_output_4_0_g353 * 0.0887 ) ) , ( ( temp_output_18_0_g353 * 0.0052 ) + ( temp_output_11_0_g353 * -0.0144 ) + ( temp_output_4_0_g353 * 1.0089 ) ) , 0.0));
			float3 Wnorldm258 = (WorldNormalVector( i , float3(0,0,1) ));
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult184_g348 = dot( Wnorldm258 , ase_worldlightDir );
			float2 temp_cast_5 = (( ( dotResult184_g348 + 1.0 ) * 0.5 )).xx;
			float ToonRamp354_g348 = tex2D( _ToonRamp_Mask, temp_cast_5 ).r;
			#ifdef _RECEIVESHADOW_ON_ON
				float staticSwitch201_g348 = ase_lightAtten;
			#else
				float staticSwitch201_g348 = 1.0;
			#endif
			float clampResult332_g348 = clamp( ( ( ToonRamp354_g348 * staticSwitch201_g348 ) + 0.0 ) , 0.0 , 1.0 );
			float temp_output_751_385 = ( tex2D( _Tex_Shadow, Standerd_UV648 ).r * clampResult332_g348 );
			float clampResult389_g350 = clamp( ( temp_output_751_385 + ( 1.0 - _Shadow_Opacity ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g350 = lerp( ( appendResult233_g350 * appendResult5_g353 ) , ( ase_lightColor * temp_output_348_0_g350 ) , clampResult389_g350);
			float4 Light_Shadow521 = lerpResult259_g350;
			float4 color604 = IsGammaSpace() ? float4(0,1,0.129231,0) : float4(0,1,0.01517276,0);
			#ifdef _MATERIAL_CHECK_ON
				float4 staticSwitch603 = color604;
			#else
				float4 staticSwitch603 = ( temp_output_64_0_g357 + Light_Shadow521 );
			#endif
			c.rgb = staticSwitch603.rgb;
			c.a = 1;
			clip( temp_output_773_0.r - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17400
2028;1;1706;1050;1142.373;3370.09;3.806452;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;622;1140.859,-1884.068;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;648;1377.989,-1886.522;Float;False;Standerd_UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;660;2797.344,-2314.353;Float;True;Property;_Tex_Decal;Tex_Decal;17;0;Create;True;0;0;False;0;None;None;False;black;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;649;2805.729,-2433.814;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;488;2797.964,-2667.594;Float;True;Property;_Tex_Base;Tex_Base;16;0;Create;True;0;0;False;0;None;a3ef2e18f4a1e8c4b94cf10958c03c08;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;487;3057.609,-2667.217;Inherit;True;Property;_TextureSample0;Texture Sample 0;19;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;775;294.8918,-2349.683;Inherit;False;Constant;_Vector1;Vector 0;19;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;659;3064.254,-2315.464;Inherit;True;Property;_TextureSample6;Texture Sample 6;58;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;183;523.1376,-2378.447;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;661;3390.168,-2334.466;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;410;3380.788,-2182.958;Float;False;Property;_MainColor_DRFA;++MainColor_DRFA++++++;7;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;414;3349.082,-2003.508;Float;False;Property;_BlendMainColor;BlendMainColor;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;655;1162.754,-1637.533;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;767.2126,-2356.374;Float;False;Wnorldm;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;413;3697.131,-2230.76;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;601;1360.702,-1659.861;Inherit;True;Property;_Tex_Shadow;Tex_Shadow;18;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;769;1506.138,-1386.733;Inherit;False;Constant;_Float1;Float 1;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;519;1442.329,-1462.4;Inherit;False;258;Wnorldm;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;528;3874.977,-2231.684;Float;False;Mtex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;751;1659.381,-1495.802;Inherit;False;CH_Light_Shadow_Sepa1;9;;348;666f4a460cae7a5459e5c899bc7bed4f;0;3;374;FLOAT;1;False;363;FLOAT3;0,0,0;False;361;FLOAT;0;False;1;FLOAT;385
Node;AmplifyShaderEditor.GetLocalVarNode;518;1854.407,-1374.293;Inherit;False;528;Mtex;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;690;2061.167,-1456.753;Inherit;False;CH_Light_Shadow_Sepa2;12;;350;7c2964b235eed694c96a4b05f6a580ea;0;2;385;FLOAT;0;False;348;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;521;2410.342,-1465.008;Float;True;Light_Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;593;3388.824,-2572.5;Float;False;Mtex_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;772;3766.533,-2038.529;Inherit;False;CH_change_eff;19;;357;0abdb2aaeb62e3645a16d32e261e24b9;0;0;2;COLOR;33;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;524;3959.412,-1838.446;Inherit;True;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;771;4086.533,-2070.528;Inherit;False;593;Mtex_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;774;4246.533,-1894.529;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;592;4278.11,-1426.464;Inherit;False;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;604;4223.979,-1739.24;Float;False;Constant;_Check_Color;Check_Color;30;0;Create;True;0;0;False;0;0,1,0.129231,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;773;4322.163,-2036.351;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;597;4293.262,-1348.123;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;776;4528.946,-1439.641;Inherit;False;CH_Outline2_V2;1;;359;196483622a3b09f4988bb1e9e142f59f;0;3;98;FLOAT;0;False;99;FLOAT4;0,0,0,0;False;100;FLOAT3;0,0,0;False;1;FLOAT3;29
Node;AmplifyShaderEditor.StaticSwitch;603;4455.31,-1823.201;Float;False;Property;_Material_Check;Material_Check;0;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;675;2064.388,-1533.691;Float;False;Light_Shadow_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4809.807,-2089.179;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Girls/Chara/CH_DR_Skin_V3_DEF;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;32;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;648;0;622;0
WireConnection;487;0;488;0
WireConnection;487;1;649;0
WireConnection;659;0;660;0
WireConnection;659;1;649;0
WireConnection;183;0;775;0
WireConnection;661;0;487;0
WireConnection;661;1;659;0
WireConnection;661;2;659;4
WireConnection;258;0;183;0
WireConnection;413;0;661;0
WireConnection;413;1;410;0
WireConnection;413;2;414;0
WireConnection;601;1;655;0
WireConnection;528;0;413;0
WireConnection;751;374;601;1
WireConnection;751;363;519;0
WireConnection;751;361;769;0
WireConnection;690;385;751;385
WireConnection;690;348;518;0
WireConnection;521;0;690;0
WireConnection;593;0;487;4
WireConnection;774;0;772;33
WireConnection;774;1;524;0
WireConnection;773;0;771;0
WireConnection;773;1;772;0
WireConnection;776;98;773;0
WireConnection;776;99;592;0
WireConnection;776;100;597;0
WireConnection;603;1;774;0
WireConnection;603;0;604;0
WireConnection;675;0;751;385
WireConnection;0;10;773;0
WireConnection;0;13;603;0
WireConnection;0;11;776;29
ASEEND*/
//CHKSM=6C08AF85112CE2A417F964687DFDF68C597D0E0B