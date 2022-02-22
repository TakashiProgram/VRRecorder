// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Girls/Chara/CH_DR_Std_V3_DEF"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_OutLine_Color("++OutLine_Color++++++", Color) = (1,1,1,1)
		_Outline_Color_Blend("Outline_Color_Blend", Range( 0 , 1)) = 0
		_Saturation_Outline("Saturation_Outline", Range( 0 , 5)) = 2.5
		_Blightness_Outline("Blightness_Outline", Range( 0 , 1)) = 0.9
		_Line_width("***Line_width***", Range( 0 , 10)) = 1
		[Toggle(_MATERIAL_CHECK_ON)] _Material_Check("Material_Check", Float) = 0
		_MainColor_DRFA("++MainColor_DRFA++++++", Color) = (1,1,1,1)
		_BlendMainColor_DRFA("BlendMainColor_DRFA", Range( 0 , 1)) = 0
		[Toggle(_MATCAP_BLEND_OVERLAY_ONLIGHTEN_OFF_ON)] _Matcap_Blend_Overlay_OnLighten_Off("++Matcap_Blend_Overlay_On / Lighten_Off++++", Float) = 0
		_Matcap_Opacity("Matcap_Opacity", Range( 0 , 1)) = 0
		_MatCap_Shadowside_Opacity("MatCap_Shadowside_Opacity", Range( 0 , 1)) = 1
		_Emi_Int_DRFA("Emi_Int_DRFA", Range( 0 , 1)) = 0
		[Toggle(_RECEIVESHADOW_ON_ON)] _ReceiveShadow_On("++ReceiveShadow_On++++", Float) = 1
		_ToonRamp_Mask("ToonRamp_Mask", 2D) = "white" {}
		_LowColor_S("LowColor_S", Range( 0 , 10)) = 2
		_LowColor_V("LowColor_V", Range( 0 , 1)) = 0.9
		_Shadow_Opacity("Shadow_Opacity", Range( 0 , 1)) = 0.5
		_Tex_Base("Tex_Base", 2D) = "white" {}
		_Tex_Decal("Tex_Decal", 2D) = "black" {}
		_Tex_MatCap("Tex_MatCap", 2D) = "white" {}
		_Tex_Shadow("Tex_Shadow", 2D) = "white" {}
		_Tex_MatcapMask("Tex_MatcapMask", 2D) = "white" {}
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
			float3 objToWorld39_g414 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float temp_output_58_0_g414 = saturate( ( ( length( ( objToWorld39_g414 - _WorldSpaceCameraPos ) ) - 0.5 ) / ( 17.0 - 0.5 ) ) );
			float temp_output_56_0_g414 = saturate( ( ( abs( ( ( atan( ( 1.0 / float4( UNITY_MATRIX_P[0][1],UNITY_MATRIX_P[1][1],UNITY_MATRIX_P[2][1],UNITY_MATRIX_P[3][1] ).y ) ) * 2.0 ) * ( 180.0 / UNITY_PI ) ) ) - 13.0 ) / ( 165.0 - 13.0 ) ) );
			float outlineVar = ( ( ( ( ( temp_output_58_0_g414 * 0.45 ) + ( ( 1.0 - 0.45 ) * temp_output_56_0_g414 ) ) * ( 0.01 - 0.0003 ) ) + 0.0003 ) * _Line_width );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float2 Standerd_UV648 = i.uv_texcoord;
			float4 tex2DNode487 = tex2D( _Tex_Base, Standerd_UV648 );
			float4 tex2DNode659 = tex2D( _Tex_Decal, Standerd_UV648 );
			float4 lerpResult661 = lerp( tex2DNode487 , tex2DNode659 , tex2DNode659.a);
			float4 lerpResult413 = lerp( lerpResult661 , _MainColor_DRFA , _BlendMainColor_DRFA);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g378 = Mtex528;
			float3 hsvTorgb198_g378 = RGBToHSV( temp_output_348_0_g378.rgb );
			float3 hsvTorgb219_g378 = HSVToRGB( float3(hsvTorgb198_g378.x,( hsvTorgb198_g378.y * _LowColor_S ),( hsvTorgb198_g378.z * _LowColor_V )) );
			float4 appendResult233_g378 = (float4(hsvTorgb219_g378 , (temp_output_348_0_g378).a));
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
			float Shadow_opa805 = _Shadow_Opacity;
			float clampResult397_g378 = clamp( ( temp_output_751_385 + ( 1.0 - Shadow_opa805 ) ) , 0.0 , 1.0 );
			float4 lerpResult392_g378 = lerp( appendResult233_g378 , temp_output_348_0_g378 , clampResult397_g378);
			float4 Light_Shadow521 = lerpResult392_g378;
			float4 temp_output_348_0_g410 = Light_Shadow521;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_24_0_g411 = ase_lightColor;
			float4 break26_g411 = temp_output_24_0_g411;
			float temp_output_217_29_g410 = ( ( break26_g411.r * 0.1769 ) + ( break26_g411.g * 0.8124 ) + ( break26_g411.b * 0.0107 ) );
			float4 temp_output_24_0_g412 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g412 = temp_output_24_0_g412;
			float temp_output_18_0_g413 = ( temp_output_217_29_g410 * pow( ( ( break28_g412.r * 0.4898 ) + ( break28_g412.g * 0.3101 ) + ( break28_g412.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g412 = temp_output_24_0_g412;
			float temp_output_11_0_g413 = ( temp_output_217_29_g410 * pow( ( ( break26_g412.r * 0.1769 ) + ( break26_g412.g * 0.8124 ) + ( break26_g412.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g412 = temp_output_24_0_g412;
			float temp_output_4_0_g413 = ( temp_output_217_29_g410 * pow( ( ( break12_g412.r * 0.0 ) + ( break12_g412.g * 0.01 ) + ( break12_g412.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g413 = (float4(( ( temp_output_18_0_g413 * 2.3655 ) + ( temp_output_11_0_g413 * -0.8971 ) + ( temp_output_4_0_g413 * -0.4683 ) ) , ( ( temp_output_18_0_g413 * -0.5151 ) + ( temp_output_11_0_g413 * 1.4264 ) + ( temp_output_4_0_g413 * 0.0887 ) ) , ( ( temp_output_18_0_g413 * 0.0052 ) + ( temp_output_11_0_g413 * -0.0144 ) + ( temp_output_4_0_g413 * 1.0089 ) ) , 0.0));
			float Light_Shadow_Alpha675 = temp_output_751_385;
			float clampResult389_g410 = clamp( ( Light_Shadow_Alpha675 + ( 1.0 - Shadow_opa805 ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g410 = lerp( ( temp_output_348_0_g410 * appendResult5_g413 ) , ( ase_lightColor * temp_output_348_0_g410 ) , clampResult389_g410);
			float3 hsvTorgb92_g414 = RGBToHSV( lerpResult259_g410.xyz );
			float3 hsvTorgb91_g414 = HSVToRGB( float3(hsvTorgb92_g414.x,( hsvTorgb92_g414.y * _Saturation_Outline ),( hsvTorgb92_g414.z * _Blightness_Outline )) );
			float4 lerpResult127_g414 = lerp( float4( hsvTorgb91_g414 , 0.0 ) , _OutLine_Color , _Outline_Color_Blend);
			float Mtex_Alpha593 = tex2DNode487.a;
			float temp_output_48_0_g405 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_8 = (( _c_Level - temp_output_48_0_g405 )).xxxx;
			float4 temp_cast_9 = (( _c_Level + temp_output_48_0_g405 )).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g405 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g405 = tex2D( _c_Gradation, appendResult39_g405 );
			float4 smoothstepResult54_g405 = smoothstep( temp_cast_8 , temp_cast_9 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g405 ) ):( tex2DNode41_g405 )) ) ));
			float4 blendOpSrc59_g405 = smoothstepResult54_g405;
			float4 blendOpDest59_g405 = ( 1.0 - smoothstepResult54_g405 );
			float4 temp_output_59_0_g405 = ( saturate( ( blendOpSrc59_g405 * blendOpDest59_g405 ) ));
			float2 temp_output_53_0_g405 = ( appendResult39_g405 * float2( 5,5 ) );
			float2 panner57_g405 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g405);
			float mulTime55_g405 = _Time.y * 0.5;
			float2 panner58_g405 = ( mulTime55_g405 * float2( 0,0 ) + temp_output_53_0_g405);
			float4 blendOpSrc64_g405 = ( temp_output_59_0_g405 + temp_output_59_0_g405 );
			float4 blendOpDest64_g405 = ( tex2D( _c_DotTex1, panner57_g405 ) * tex2D( _c_DotTex1, panner58_g405 ) );
			float4 temp_output_64_0_g405 = ( saturate( ( blendOpSrc64_g405 * blendOpDest64_g405 ) ));
			float4 clampResult69_g405 = clamp( ( smoothstepResult54_g405 + temp_output_64_0_g405 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_774_0 = ( Mtex_Alpha593 * clampResult69_g405 );
			o.Emission = lerpResult127_g414.rgb;
			clip( temp_output_774_0.r - _Cutoff );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _MATERIAL_CHECK_ON
		#pragma shader_feature _RECEIVESHADOW_ON_ON
		#pragma shader_feature _MATCAP_BLEND_OVERLAY_ONLIGHTEN_OFF_ON
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

		uniform float _Emi_Int_DRFA;
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
		uniform float _BlendMainColor_DRFA;
		uniform float _LowColor_S;
		uniform float _LowColor_V;
		uniform sampler2D _Tex_Shadow;
		uniform sampler2D _ToonRamp_Mask;
		uniform float _Shadow_Opacity;
		uniform sampler2D _Tex_MatCap;
		uniform float _Matcap_Opacity;
		uniform float _MatCap_Shadowside_Opacity;
		uniform sampler2D _Tex_MatcapMask;
		uniform float4 _Tex_MatcapMask_ST;
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
			float temp_output_48_0_g405 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_1 = (( _c_Level - temp_output_48_0_g405 )).xxxx;
			float4 temp_cast_2 = (( _c_Level + temp_output_48_0_g405 )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g405 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g405 = tex2D( _c_Gradation, appendResult39_g405 );
			float4 smoothstepResult54_g405 = smoothstep( temp_cast_1 , temp_cast_2 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g405 ) ):( tex2DNode41_g405 )) ) ));
			float4 blendOpSrc59_g405 = smoothstepResult54_g405;
			float4 blendOpDest59_g405 = ( 1.0 - smoothstepResult54_g405 );
			float4 temp_output_59_0_g405 = ( saturate( ( blendOpSrc59_g405 * blendOpDest59_g405 ) ));
			float2 temp_output_53_0_g405 = ( appendResult39_g405 * float2( 5,5 ) );
			float2 panner57_g405 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g405);
			float mulTime55_g405 = _Time.y * 0.5;
			float2 panner58_g405 = ( mulTime55_g405 * float2( 0,0 ) + temp_output_53_0_g405);
			float4 blendOpSrc64_g405 = ( temp_output_59_0_g405 + temp_output_59_0_g405 );
			float4 blendOpDest64_g405 = ( tex2D( _c_DotTex1, panner57_g405 ) * tex2D( _c_DotTex1, panner58_g405 ) );
			float4 temp_output_64_0_g405 = ( saturate( ( blendOpSrc64_g405 * blendOpDest64_g405 ) ));
			float4 clampResult69_g405 = clamp( ( smoothstepResult54_g405 + temp_output_64_0_g405 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_774_0 = ( Mtex_Alpha593 * clampResult69_g405 );
			float4 tex2DNode659 = tex2D( _Tex_Decal, Standerd_UV648 );
			float4 lerpResult661 = lerp( tex2DNode487 , tex2DNode659 , tex2DNode659.a);
			float4 lerpResult413 = lerp( lerpResult661 , _MainColor_DRFA , _BlendMainColor_DRFA);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g378 = Mtex528;
			float3 hsvTorgb198_g378 = RGBToHSV( temp_output_348_0_g378.rgb );
			float3 hsvTorgb219_g378 = HSVToRGB( float3(hsvTorgb198_g378.x,( hsvTorgb198_g378.y * _LowColor_S ),( hsvTorgb198_g378.z * _LowColor_V )) );
			float4 appendResult233_g378 = (float4(hsvTorgb219_g378 , (temp_output_348_0_g378).a));
			float3 Wnorldm258 = (WorldNormalVector( i , float3(0,0,1) ));
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult184_g348 = dot( Wnorldm258 , ase_worldlightDir );
			float2 temp_cast_6 = (( ( dotResult184_g348 + 1.0 ) * 0.5 )).xx;
			float ToonRamp354_g348 = tex2D( _ToonRamp_Mask, temp_cast_6 ).r;
			#ifdef _RECEIVESHADOW_ON_ON
				float staticSwitch201_g348 = ase_lightAtten;
			#else
				float staticSwitch201_g348 = 1.0;
			#endif
			float clampResult332_g348 = clamp( ( ( ToonRamp354_g348 * staticSwitch201_g348 ) + 0.0 ) , 0.0 , 1.0 );
			float temp_output_751_385 = ( tex2D( _Tex_Shadow, Standerd_UV648 ).r * clampResult332_g348 );
			float Shadow_opa805 = _Shadow_Opacity;
			float clampResult397_g378 = clamp( ( temp_output_751_385 + ( 1.0 - Shadow_opa805 ) ) , 0.0 , 1.0 );
			float4 lerpResult392_g378 = lerp( appendResult233_g378 , temp_output_348_0_g378 , clampResult397_g378);
			float4 Light_Shadow521 = lerpResult392_g378;
			float3 ViewNormal278 = ( ( mul( UNITY_MATRIX_V, float4( Wnorldm258 , 0.0 ) ).xyz * 0.5 ) + 0.5 );
			float4 Matcap281 = tex2D( _Tex_MatCap, ViewNormal278.xy );
			float4 blendOpSrc789 = Light_Shadow521;
			float4 blendOpDest789 = Matcap281;
			float4 blendOpSrc790 = Light_Shadow521;
			float4 blendOpDest790 = Matcap281;
			#ifdef _MATCAP_BLEND_OVERLAY_ONLIGHTEN_OFF_ON
				float4 staticSwitch792 = ( saturate( (( blendOpDest790 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest790 ) * ( 1.0 - blendOpSrc790 ) ) : ( 2.0 * blendOpDest790 * blendOpSrc790 ) ) ));
			#else
				float4 staticSwitch792 = ( saturate( 	max( blendOpSrc789, blendOpDest789 ) ));
			#endif
			float Light_Shadow_Alpha675 = temp_output_751_385;
			float2 uv_Tex_MatcapMask = i.uv_texcoord * _Tex_MatcapMask_ST.xy + _Tex_MatcapMask_ST.zw;
			float4 lerpResult794 = lerp( Light_Shadow521 , staticSwitch792 , ( _Matcap_Opacity * ( 1.0 - ( ( 1.0 - _MatCap_Shadowside_Opacity ) * ( 1.0 - Light_Shadow_Alpha675 ) ) ) * tex2D( _Tex_MatcapMask, uv_Tex_MatcapMask ).r ));
			float4 Mtex_MC795 = lerpResult794;
			float4 temp_output_348_0_g406 = Mtex_MC795;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_24_0_g407 = ase_lightColor;
			float4 break26_g407 = temp_output_24_0_g407;
			float temp_output_217_29_g406 = ( ( break26_g407.r * 0.1769 ) + ( break26_g407.g * 0.8124 ) + ( break26_g407.b * 0.0107 ) );
			float4 temp_output_24_0_g408 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g408 = temp_output_24_0_g408;
			float temp_output_18_0_g409 = ( temp_output_217_29_g406 * pow( ( ( break28_g408.r * 0.4898 ) + ( break28_g408.g * 0.3101 ) + ( break28_g408.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g408 = temp_output_24_0_g408;
			float temp_output_11_0_g409 = ( temp_output_217_29_g406 * pow( ( ( break26_g408.r * 0.1769 ) + ( break26_g408.g * 0.8124 ) + ( break26_g408.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g408 = temp_output_24_0_g408;
			float temp_output_4_0_g409 = ( temp_output_217_29_g406 * pow( ( ( break12_g408.r * 0.0 ) + ( break12_g408.g * 0.01 ) + ( break12_g408.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g409 = (float4(( ( temp_output_18_0_g409 * 2.3655 ) + ( temp_output_11_0_g409 * -0.8971 ) + ( temp_output_4_0_g409 * -0.4683 ) ) , ( ( temp_output_18_0_g409 * -0.5151 ) + ( temp_output_11_0_g409 * 1.4264 ) + ( temp_output_4_0_g409 * 0.0887 ) ) , ( ( temp_output_18_0_g409 * 0.0052 ) + ( temp_output_11_0_g409 * -0.0144 ) + ( temp_output_4_0_g409 * 1.0089 ) ) , 0.0));
			float clampResult389_g406 = clamp( ( Light_Shadow_Alpha675 + ( 1.0 - Shadow_opa805 ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g406 = lerp( ( temp_output_348_0_g406 * appendResult5_g409 ) , ( ase_lightColor * temp_output_348_0_g406 ) , clampResult389_g406);
			float4 color604 = IsGammaSpace() ? float4(0,1,0.129231,0) : float4(0,1,0.01517276,0);
			#ifdef _MATERIAL_CHECK_ON
				float4 staticSwitch603 = color604;
			#else
				float4 staticSwitch603 = ( temp_output_64_0_g405 + lerpResult259_g406 );
			#endif
			c.rgb = staticSwitch603.rgb;
			c.a = 1;
			clip( temp_output_774_0.r - _Cutoff );
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
			float3 temp_cast_0 = (_Emi_Int_DRFA).xxx;
			o.Emission = temp_cast_0;
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
2028;1;1706;1050;-505.1563;3037.587;3.488087;True;True
Node;AmplifyShaderEditor.Vector3Node;803;330.0598,-2384.07;Inherit;False;Constant;_Vector1;Vector 0;19;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;183;523.1376,-2378.447;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;622;1140.859,-1884.068;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;767.2126,-2356.374;Float;False;Wnorldm;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;648;1377.989,-1886.522;Float;False;Standerd_UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;655;1162.754,-1637.533;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;488;2797.964,-2667.594;Float;True;Property;_Tex_Base;Tex_Base;20;0;Create;True;0;0;False;0;None;a3ef2e18f4a1e8c4b94cf10958c03c08;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;660;2797.344,-2314.353;Float;True;Property;_Tex_Decal;Tex_Decal;21;0;Create;True;0;0;False;0;None;None;False;black;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ViewMatrixNode;272;1207.532,-2256.668;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;1147.642,-2157.681;Inherit;False;258;Wnorldm;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;649;2805.729,-2433.814;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;601;1360.702,-1659.861;Inherit;True;Property;_Tex_Shadow;Tex_Shadow;23;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;519;1442.329,-1462.4;Inherit;False;258;Wnorldm;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;487;3057.609,-2667.217;Inherit;True;Property;_TextureSample0;Texture Sample 0;19;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;769;1506.138,-1386.733;Inherit;False;Constant;_Float1;Float 1;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;659;3064.254,-2315.464;Inherit;True;Property;_TextureSample6;Texture Sample 6;58;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;1342.9,-2226.305;Inherit;True;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;282;1509.411,-2015.446;Float;False;Constant;_Float15;Float 15;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;1618.261,-2185.158;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;751;1659.381,-1495.802;Inherit;False;CH_Light_Shadow_Sepa1;13;;348;666f4a460cae7a5459e5c899bc7bed4f;0;3;374;FLOAT;1;False;363;FLOAT3;0,0,0;False;361;FLOAT;0;False;1;FLOAT;385
Node;AmplifyShaderEditor.ColorNode;410;3349.314,-2182.958;Float;False;Property;_MainColor_DRFA;++MainColor_DRFA++++++;7;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;661;3389.593,-2415.251;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;414;3349.082,-2003.508;Float;False;Property;_BlendMainColor_DRFA;BlendMainColor_DRFA;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;276;1770.556,-2136.413;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;413;3652.748,-2199.885;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;675;2064.388,-1533.691;Float;False;Light_Shadow_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;777;2547.677,-829.8096;Inherit;False;675;Light_Shadow_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;804;1587.629,-1200.037;Inherit;False;Property;_Shadow_Opacity;Shadow_Opacity;19;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;278;2015.372,-2130.754;Float;False;ViewNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;778;2510.871,-958.2009;Float;False;Property;_MatCap_Shadowside_Opacity;MatCap_Shadowside_Opacity;11;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;528;3830.594,-2218.752;Float;False;Mtex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;518;1854.407,-1374.293;Inherit;False;528;Mtex;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;779;2818.518,-953.8198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;780;2811.767,-825.5527;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;283;1182.149,-2472.958;Inherit;False;278;ViewNormal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;279;1163.695,-2674.797;Float;True;Property;_Tex_MatCap;Tex_MatCap;22;0;Create;True;0;0;False;0;None;2ed438bce1fdc30449f85baf9f25e737;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;805;1922.381,-1205.632;Inherit;False;Shadow_opa;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;776;2062.795,-1450.868;Inherit;False;CH_Light_Shadow_Sepa1_5;16;;378;8a054440d69eafe4d86d21965b4c82d7;0;3;398;FLOAT;0;False;348;COLOR;0,0,0,0;False;396;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;782;2975.476,-955.5078;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;781;2803.694,-729.2436;Float;True;Property;_Tex_MatcapMask;Tex_MatcapMask;24;0;Create;False;0;0;False;0;None;fe9233ff2c4c73046a2c8511ec3db430;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;280;1422.506,-2614.143;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;281;1726.42,-2614.501;Float;False;Matcap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;785;3047.424,-728.0106;Inherit;True;Property;_Tex_BlendMask1;Tex_BlendMask;43;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;783;3191.504,-953.8198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;784;3067.07,-1052.238;Float;False;Property;_Matcap_Opacity;Matcap_Opacity;10;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;521;2410.342,-1465.008;Float;False;Light_Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;786;2811.324,-1416.798;Inherit;False;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;788;3410.926,-1085.333;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;787;2807.913,-1188.908;Inherit;False;281;Matcap;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;791;3734.728,-1144.136;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;790;3103.653,-1209.603;Inherit;False;Overlay;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;789;3094.256,-1346.288;Inherit;False;Lighten;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;792;3337.423,-1322.785;Float;False;Property;_Matcap_Blend_Overlay_OnLighten_Off;++Matcap_Blend_Overlay_On / Lighten_Off++++;9;0;Create;False;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;793;3753.146,-1273.063;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;794;3789.262,-1400.316;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;795;3946.958,-1420.936;Inherit;False;Mtex_MC;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;797;3702.878,-1625.063;Inherit;False;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;799;3719.752,-1723.846;Inherit;False;795;Mtex_MC;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;806;3704.538,-1541.965;Inherit;False;805;Shadow_opa;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;796;3691.03,-1797.124;Inherit;False;675;Light_Shadow_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;593;3388.824,-2572.5;Float;False;Mtex_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;798;3993.318,-1667.497;Inherit;False;CH_Light_Shadow_Sepa2_0;-1;;410;5a94b50823288e143a8cd2c1c330b6a5;0;3;385;FLOAT;0;False;348;COLOR;0,0,0,0;False;391;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;773;4272,-1968;Inherit;False;593;Mtex_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;772;3952,-1936;Inherit;False;CH_change_eff;25;;405;0abdb2aaeb62e3645a16d32e261e24b9;0;0;2;COLOR;33;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;800;3987.817,-1771.672;Inherit;False;CH_Light_Shadow_Sepa2_0;-1;;406;5a94b50823288e143a8cd2c1c330b6a5;0;3;385;FLOAT;0;False;348;COLOR;0,0,0,0;False;391;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;775;4432,-1792;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;774;4507.629,-1933.822;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;604;4653.228,-1718.636;Float;False;Constant;_Check_Color;Check_Color;30;0;Create;True;0;0;False;0;0,1,0.129231,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;802;4584.24,-1378.642;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;597;4596.677,-1297.284;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;771;4861.727,-2111.429;Inherit;False;Property;_Emi_Int_DRFA;Emi_Int_DRFA;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;603;4884.559,-1802.597;Float;False;Property;_Material_Check;Material_Check;6;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;807;4870.661,-1383.985;Inherit;False;CH_Outline2_V2;0;;414;196483622a3b09f4988bb1e9e142f59f;0;3;98;FLOAT;0;False;99;FLOAT4;0,0,0,0;False;100;FLOAT3;0,0,0;False;1;FLOAT3;29
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5219.283,-2145.704;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Girls/Chara/CH_DR_Std_V3_DEF;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;38;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;183;0;803;0
WireConnection;258;0;183;0
WireConnection;648;0;622;0
WireConnection;601;1;655;0
WireConnection;487;0;488;0
WireConnection;487;1;649;0
WireConnection;659;0;660;0
WireConnection;659;1;649;0
WireConnection;274;0;272;0
WireConnection;274;1;346;0
WireConnection;275;0;274;0
WireConnection;275;1;282;0
WireConnection;751;374;601;1
WireConnection;751;363;519;0
WireConnection;751;361;769;0
WireConnection;661;0;487;0
WireConnection;661;1;659;0
WireConnection;661;2;659;4
WireConnection;276;0;275;0
WireConnection;276;1;282;0
WireConnection;413;0;661;0
WireConnection;413;1;410;0
WireConnection;413;2;414;0
WireConnection;675;0;751;385
WireConnection;278;0;276;0
WireConnection;528;0;413;0
WireConnection;779;0;778;0
WireConnection;780;0;777;0
WireConnection;805;0;804;0
WireConnection;776;398;751;385
WireConnection;776;348;518;0
WireConnection;776;396;805;0
WireConnection;782;0;779;0
WireConnection;782;1;780;0
WireConnection;280;0;279;0
WireConnection;280;1;283;0
WireConnection;281;0;280;0
WireConnection;785;0;781;0
WireConnection;783;0;782;0
WireConnection;521;0;776;0
WireConnection;788;0;784;0
WireConnection;788;1;783;0
WireConnection;788;2;785;1
WireConnection;791;0;788;0
WireConnection;790;0;786;0
WireConnection;790;1;787;0
WireConnection;789;0;786;0
WireConnection;789;1;787;0
WireConnection;792;1;789;0
WireConnection;792;0;790;0
WireConnection;793;0;791;0
WireConnection;794;0;786;0
WireConnection;794;1;792;0
WireConnection;794;2;793;0
WireConnection;795;0;794;0
WireConnection;593;0;487;4
WireConnection;798;385;796;0
WireConnection;798;348;797;0
WireConnection;798;391;806;0
WireConnection;800;385;796;0
WireConnection;800;348;799;0
WireConnection;800;391;806;0
WireConnection;775;0;772;33
WireConnection;775;1;800;0
WireConnection;774;0;773;0
WireConnection;774;1;772;0
WireConnection;802;0;798;0
WireConnection;603;1;775;0
WireConnection;603;0;604;0
WireConnection;807;98;774;0
WireConnection;807;99;802;0
WireConnection;807;100;597;0
WireConnection;0;2;771;0
WireConnection;0;10;774;0
WireConnection;0;13;603;0
WireConnection;0;11;807;29
ASEEND*/
//CHKSM=D29A03415721676EE94037184577D4ECA8F7564D