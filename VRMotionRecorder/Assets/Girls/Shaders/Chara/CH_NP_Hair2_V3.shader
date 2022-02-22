// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Girls/Chara/CH_NP_Hair2_V3"
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
		_MainColor("++MainColor++++++", Color) = (1,1,1,1)
		_BlendMainColor("BlendMainColor", Range( 0 , 1)) = 0
		_Matcap_Opacity("++Matcap_Opacity+++++++", Range( 0 , 1)) = 0
		_MatCap_Shadowside_Opacity("MatCap_Shadowside_Opacity", Range( 0 , 1)) = 1
		_Specular_Color("++Specular_Color++++", Color) = (0,0,0,1)
		_Mesh_Specular_Color("Mesh_Specular_Color", Color) = (0,0,0,1)
		_Specular_Int("Specular_Int", Range( 0 , 1)) = 1
		_Shininess("Shininess", Range( 0.01 , 1)) = 0.1
		[Toggle(_RECEIVESHADOW_ON_ON)] _ReceiveShadow_On("++ReceiveShadow_On++++", Float) = 1
		_ToonRamp_Mask("ToonRamp_Mask", 2D) = "white" {}
		_LowColor_H("LowColor_H", Range( 0 , 1)) = 0
		_LowColor_S("LowColor_S", Range( 0 , 10)) = 2
		_LowColor_V("LowColor_V", Range( 0 , 1)) = 0.9
		_Mesh_LowColor_H("Mesh_LowColor_H", Range( 0 , 1)) = 0
		_Mesh_LowColor_S("Mesh_LowColor_S", Range( 0 , 10)) = 2
		_Mesh_LowColor_V("Mesh_LowColor_V", Range( 0 , 1)) = 0.9
		_Shadow_Opacity("Shadow_Opacity", Range( 0 , 1)) = 0.5
		_Tex_Base("Tex_Base", 2D) = "white" {}
		_Tex_MultiMask("Tex_MultiMask", 2D) = "white" {}
		_Tex_MatCap("Tex_MatCap", 2D) = "white" {}
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
			float3 objToWorld39_g439 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float temp_output_58_0_g439 = saturate( ( ( length( ( objToWorld39_g439 - _WorldSpaceCameraPos ) ) - 0.5 ) / ( 17.0 - 0.5 ) ) );
			float temp_output_56_0_g439 = saturate( ( ( abs( ( ( atan( ( 1.0 / float4( UNITY_MATRIX_P[0][1],UNITY_MATRIX_P[1][1],UNITY_MATRIX_P[2][1],UNITY_MATRIX_P[3][1] ).y ) ) * 2.0 ) * ( 180.0 / UNITY_PI ) ) ) - 13.0 ) / ( 165.0 - 13.0 ) ) );
			float outlineVar = ( ( ( ( ( temp_output_58_0_g439 * 0.45 ) + ( ( 1.0 - 0.45 ) * temp_output_56_0_g439 ) ) * ( 0.01 - 0.0003 ) ) + 0.0003 ) * _Line_width );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float2 Standerd_UV648 = i.uv_texcoord;
			float4 tex2DNode487 = tex2D( _Tex_Base, Standerd_UV648 );
			float4 lerpResult413 = lerp( tex2DNode487 , _MainColor , _BlendMainColor);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g395 = Mtex528;
			float3 hsvTorgb198_g395 = RGBToHSV( temp_output_348_0_g395.rgb );
			float3 hsvTorgb219_g395 = HSVToRGB( float3(( hsvTorgb198_g395.x + _LowColor_H ),( hsvTorgb198_g395.y * _LowColor_S ),( hsvTorgb198_g395.z * _LowColor_V )) );
			float3 hsvTorgb393_g395 = HSVToRGB( float3(( hsvTorgb198_g395.x + _Mesh_LowColor_H ),( hsvTorgb198_g395.y * _Mesh_LowColor_S ),( hsvTorgb198_g395.z * _Mesh_LowColor_V )) );
			float4 tex2DNode957 = tex2D( _Tex_MultiMask, Standerd_UV648 );
			float Mesh_Mask898 = tex2DNode957.r;
			float3 lerpResult401_g395 = lerp( hsvTorgb219_g395 , hsvTorgb393_g395 , Mesh_Mask898);
			float4 appendResult233_g395 = (float4(lerpResult401_g395 , (temp_output_348_0_g395).a));
			float Multi_TexShadow959 = tex2DNode957.b;
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
			float temp_output_751_385 = ( Multi_TexShadow959 * clampResult332_g348 );
			float Shadow_opa964 = _Shadow_Opacity;
			float clampResult408_g395 = clamp( ( temp_output_751_385 + ( 1.0 - Shadow_opa964 ) ) , 0.0 , 1.0 );
			float4 lerpResult407_g395 = lerp( appendResult233_g395 , temp_output_348_0_g395 , clampResult408_g395);
			float4 Light_Shadow521 = lerpResult407_g395;
			float4 temp_output_348_0_g426 = Light_Shadow521;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_24_0_g427 = ase_lightColor;
			float4 break26_g427 = temp_output_24_0_g427;
			float temp_output_217_29_g426 = ( ( break26_g427.r * 0.1769 ) + ( break26_g427.g * 0.8124 ) + ( break26_g427.b * 0.0107 ) );
			float4 temp_output_24_0_g428 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g428 = temp_output_24_0_g428;
			float temp_output_18_0_g429 = ( temp_output_217_29_g426 * pow( ( ( break28_g428.r * 0.4898 ) + ( break28_g428.g * 0.3101 ) + ( break28_g428.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g428 = temp_output_24_0_g428;
			float temp_output_11_0_g429 = ( temp_output_217_29_g426 * pow( ( ( break26_g428.r * 0.1769 ) + ( break26_g428.g * 0.8124 ) + ( break26_g428.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g428 = temp_output_24_0_g428;
			float temp_output_4_0_g429 = ( temp_output_217_29_g426 * pow( ( ( break12_g428.r * 0.0 ) + ( break12_g428.g * 0.01 ) + ( break12_g428.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g429 = (float4(( ( temp_output_18_0_g429 * 2.3655 ) + ( temp_output_11_0_g429 * -0.8971 ) + ( temp_output_4_0_g429 * -0.4683 ) ) , ( ( temp_output_18_0_g429 * -0.5151 ) + ( temp_output_11_0_g429 * 1.4264 ) + ( temp_output_4_0_g429 * 0.0887 ) ) , ( ( temp_output_18_0_g429 * 0.0052 ) + ( temp_output_11_0_g429 * -0.0144 ) + ( temp_output_4_0_g429 * 1.0089 ) ) , 0.0));
			float Light_Shadow_Alpha675 = temp_output_751_385;
			float clampResult389_g426 = clamp( ( Light_Shadow_Alpha675 + ( 1.0 - Shadow_opa964 ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g426 = lerp( ( temp_output_348_0_g426 * appendResult5_g429 ) , ( ase_lightColor * temp_output_348_0_g426 ) , clampResult389_g426);
			float3 hsvTorgb92_g439 = RGBToHSV( lerpResult259_g426.xyz );
			float3 hsvTorgb91_g439 = HSVToRGB( float3(hsvTorgb92_g439.x,( hsvTorgb92_g439.y * _Saturation_Outline ),( hsvTorgb92_g439.z * _Blightness_Outline )) );
			float4 lerpResult127_g439 = lerp( float4( hsvTorgb91_g439 , 0.0 ) , _OutLine_Color , _Outline_Color_Blend);
			float Mtex_Alpha593 = tex2DNode487.a;
			float temp_output_48_0_g434 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_8 = (( _c_Level - temp_output_48_0_g434 )).xxxx;
			float4 temp_cast_9 = (( _c_Level + temp_output_48_0_g434 )).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g434 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g434 = tex2D( _c_Gradation, appendResult39_g434 );
			float4 smoothstepResult54_g434 = smoothstep( temp_cast_8 , temp_cast_9 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g434 ) ):( tex2DNode41_g434 )) ) ));
			float4 blendOpSrc59_g434 = smoothstepResult54_g434;
			float4 blendOpDest59_g434 = ( 1.0 - smoothstepResult54_g434 );
			float4 temp_output_59_0_g434 = ( saturate( ( blendOpSrc59_g434 * blendOpDest59_g434 ) ));
			float2 temp_output_53_0_g434 = ( appendResult39_g434 * float2( 5,5 ) );
			float2 panner57_g434 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g434);
			float mulTime55_g434 = _Time.y * 0.5;
			float2 panner58_g434 = ( mulTime55_g434 * float2( 0,0 ) + temp_output_53_0_g434);
			float4 blendOpSrc64_g434 = ( temp_output_59_0_g434 + temp_output_59_0_g434 );
			float4 blendOpDest64_g434 = ( tex2D( _c_DotTex1, panner57_g434 ) * tex2D( _c_DotTex1, panner58_g434 ) );
			float4 temp_output_64_0_g434 = ( saturate( ( blendOpSrc64_g434 * blendOpDest64_g434 ) ));
			float4 clampResult69_g434 = clamp( ( smoothstepResult54_g434 + temp_output_64_0_g434 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_892_0 = ( Mtex_Alpha593 * clampResult69_g434 );
			o.Emission = lerpResult127_g439.rgb;
			clip( temp_output_892_0.r - _Cutoff );
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
		uniform float4 _MainColor;
		uniform float _BlendMainColor;
		uniform float _LowColor_H;
		uniform float _LowColor_S;
		uniform float _LowColor_V;
		uniform float _Mesh_LowColor_H;
		uniform float _Mesh_LowColor_S;
		uniform float _Mesh_LowColor_V;
		uniform sampler2D _Tex_MultiMask;
		uniform sampler2D _ToonRamp_Mask;
		uniform float _Shadow_Opacity;
		uniform float4 _Specular_Color;
		uniform float4 _Mesh_Specular_Color;
		uniform sampler2D _Tex_MatCap;
		uniform float _Matcap_Opacity;
		uniform float _MatCap_Shadowside_Opacity;
		uniform float _Specular_Int;
		uniform float _Shininess;
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
			float temp_output_48_0_g434 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_0 = (( _c_Level - temp_output_48_0_g434 )).xxxx;
			float4 temp_cast_1 = (( _c_Level + temp_output_48_0_g434 )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g434 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g434 = tex2D( _c_Gradation, appendResult39_g434 );
			float4 smoothstepResult54_g434 = smoothstep( temp_cast_0 , temp_cast_1 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g434 ) ):( tex2DNode41_g434 )) ) ));
			float4 blendOpSrc59_g434 = smoothstepResult54_g434;
			float4 blendOpDest59_g434 = ( 1.0 - smoothstepResult54_g434 );
			float4 temp_output_59_0_g434 = ( saturate( ( blendOpSrc59_g434 * blendOpDest59_g434 ) ));
			float2 temp_output_53_0_g434 = ( appendResult39_g434 * float2( 5,5 ) );
			float2 panner57_g434 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g434);
			float mulTime55_g434 = _Time.y * 0.5;
			float2 panner58_g434 = ( mulTime55_g434 * float2( 0,0 ) + temp_output_53_0_g434);
			float4 blendOpSrc64_g434 = ( temp_output_59_0_g434 + temp_output_59_0_g434 );
			float4 blendOpDest64_g434 = ( tex2D( _c_DotTex1, panner57_g434 ) * tex2D( _c_DotTex1, panner58_g434 ) );
			float4 temp_output_64_0_g434 = ( saturate( ( blendOpSrc64_g434 * blendOpDest64_g434 ) ));
			float4 clampResult69_g434 = clamp( ( smoothstepResult54_g434 + temp_output_64_0_g434 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 temp_output_892_0 = ( Mtex_Alpha593 * clampResult69_g434 );
			float4 lerpResult413 = lerp( tex2DNode487 , _MainColor , _BlendMainColor);
			float4 Mtex528 = lerpResult413;
			float4 temp_output_348_0_g395 = Mtex528;
			float3 hsvTorgb198_g395 = RGBToHSV( temp_output_348_0_g395.rgb );
			float3 hsvTorgb219_g395 = HSVToRGB( float3(( hsvTorgb198_g395.x + _LowColor_H ),( hsvTorgb198_g395.y * _LowColor_S ),( hsvTorgb198_g395.z * _LowColor_V )) );
			float3 hsvTorgb393_g395 = HSVToRGB( float3(( hsvTorgb198_g395.x + _Mesh_LowColor_H ),( hsvTorgb198_g395.y * _Mesh_LowColor_S ),( hsvTorgb198_g395.z * _Mesh_LowColor_V )) );
			float4 tex2DNode957 = tex2D( _Tex_MultiMask, Standerd_UV648 );
			float Mesh_Mask898 = tex2DNode957.r;
			float3 lerpResult401_g395 = lerp( hsvTorgb219_g395 , hsvTorgb393_g395 , Mesh_Mask898);
			float4 appendResult233_g395 = (float4(lerpResult401_g395 , (temp_output_348_0_g395).a));
			float Multi_TexShadow959 = tex2DNode957.b;
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
			float temp_output_751_385 = ( Multi_TexShadow959 * clampResult332_g348 );
			float Shadow_opa964 = _Shadow_Opacity;
			float clampResult408_g395 = clamp( ( temp_output_751_385 + ( 1.0 - Shadow_opa964 ) ) , 0.0 , 1.0 );
			float4 lerpResult407_g395 = lerp( appendResult233_g395 , temp_output_348_0_g395 , clampResult408_g395);
			float4 Light_Shadow521 = lerpResult407_g395;
			float3 hsvTorgb923 = RGBToHSV( _Specular_Color.rgb );
			float3 hsvTorgb926 = HSVToRGB( float3(hsvTorgb923.x,hsvTorgb923.y,1.0) );
			float3 hsvTorgb924 = RGBToHSV( _Mesh_Specular_Color.rgb );
			float3 hsvTorgb927 = HSVToRGB( float3(hsvTorgb924.x,hsvTorgb924.y,1.0) );
			float3 lerpResult904 = lerp( hsvTorgb926 , hsvTorgb927 , Mesh_Mask898);
			float3 Mesh_Spec_Color933 = lerpResult904;
			float Specular_Maskmap942 = tex2DNode957.g;
			float lerpResult925 = lerp( hsvTorgb923.z , hsvTorgb924.z , Mesh_Mask898);
			float Mesh_Spec_Value935 = lerpResult925;
			float3 ViewNormal278 = ( ( mul( UNITY_MATRIX_V, float4( Wnorldm258 , 0.0 ) ).xyz * 0.5 ) + 0.5 );
			float4 Matcap281 = tex2D( _Tex_MatCap, ViewNormal278.xy );
			float Light_Shadow_Alpha675 = temp_output_751_385;
			float4 color912 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 temp_output_43_0_g373 = color912;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 normalizeResult4_g374 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float3 normalizeResult64_g373 = normalize( (WorldNormalVector( i , float3(0,0,1) )) );
			float dotResult19_g373 = dot( normalizeResult4_g374 , normalizeResult64_g373 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_40_0_g373 = ( ase_lightColor * ase_lightAtten );
			float dotResult14_g373 = dot( normalizeResult64_g373 , ase_worldlightDir );
			UnityGI gi34_g373 = gi;
			float3 diffNorm34_g373 = normalizeResult64_g373;
			gi34_g373 = UnityGI_Base( data, 1, diffNorm34_g373 );
			float3 indirectDiffuse34_g373 = gi34_g373.indirect.diffuse + diffNorm34_g373 * 0.0001;
			float4 color676 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
			float4 temp_output_42_0_g373 = color676;
			float4 temp_output_626_0 = ( ( float4( (temp_output_43_0_g373).rgb , 0.0 ) * (temp_output_43_0_g373).a * pow( max( dotResult19_g373 , 0.0 ) , ( _Shininess * 128.0 ) ) * temp_output_40_0_g373 ) + ( ( ( temp_output_40_0_g373 * max( dotResult14_g373 , 0.0 ) ) + float4( indirectDiffuse34_g373 , 0.0 ) ) * float4( (temp_output_42_0_g373).rgb , 0.0 ) ) );
			float4 Specular_Map753 = ( _Specular_Int * temp_output_626_0 * temp_output_626_0 );
			float4 lerpResult489 = lerp( Light_Shadow521 , float4( Mesh_Spec_Color933 , 0.0 ) , ( Specular_Maskmap942 * Mesh_Spec_Value935 * ( ( Matcap281 * _Matcap_Opacity * ( 1.0 - ( ( 1.0 - _MatCap_Shadowside_Opacity ) * ( 1.0 - Light_Shadow_Alpha675 ) ) ) ) + Specular_Map753 ) ));
			float4 Mtex_by_Rim863 = lerpResult489;
			float4 temp_output_348_0_g430 = Mtex_by_Rim863;
			float4 temp_output_24_0_g431 = ase_lightColor;
			float4 break26_g431 = temp_output_24_0_g431;
			float temp_output_217_29_g430 = ( ( break26_g431.r * 0.1769 ) + ( break26_g431.g * 0.8124 ) + ( break26_g431.b * 0.0107 ) );
			float4 temp_output_24_0_g432 = UNITY_LIGHTMODEL_AMBIENT;
			float4 break28_g432 = temp_output_24_0_g432;
			float temp_output_18_0_g433 = ( temp_output_217_29_g430 * pow( ( ( break28_g432.r * 0.4898 ) + ( break28_g432.g * 0.3101 ) + ( break28_g432.b * 0.2001 ) ) , 1.5 ) );
			float4 break26_g432 = temp_output_24_0_g432;
			float temp_output_11_0_g433 = ( temp_output_217_29_g430 * pow( ( ( break26_g432.r * 0.1769 ) + ( break26_g432.g * 0.8124 ) + ( break26_g432.b * 0.0107 ) ) , 1.5 ) );
			float4 break12_g432 = temp_output_24_0_g432;
			float temp_output_4_0_g433 = ( temp_output_217_29_g430 * pow( ( ( break12_g432.r * 0.0 ) + ( break12_g432.g * 0.01 ) + ( break12_g432.b * 0.9903 ) ) , 1.5 ) );
			float4 appendResult5_g433 = (float4(( ( temp_output_18_0_g433 * 2.3655 ) + ( temp_output_11_0_g433 * -0.8971 ) + ( temp_output_4_0_g433 * -0.4683 ) ) , ( ( temp_output_18_0_g433 * -0.5151 ) + ( temp_output_11_0_g433 * 1.4264 ) + ( temp_output_4_0_g433 * 0.0887 ) ) , ( ( temp_output_18_0_g433 * 0.0052 ) + ( temp_output_11_0_g433 * -0.0144 ) + ( temp_output_4_0_g433 * 1.0089 ) ) , 0.0));
			float clampResult389_g430 = clamp( ( Light_Shadow_Alpha675 + ( 1.0 - Shadow_opa964 ) ) , 0.0 , 1.0 );
			float4 lerpResult259_g430 = lerp( ( temp_output_348_0_g430 * appendResult5_g433 ) , ( ase_lightColor * temp_output_348_0_g430 ) , clampResult389_g430);
			float4 color604 = IsGammaSpace() ? float4(0,1,0.129231,0) : float4(0,1,0.01517276,0);
			#ifdef _MATERIAL_CHECK_ON
				float4 staticSwitch603 = color604;
			#else
				float4 staticSwitch603 = ( temp_output_64_0_g434 + lerpResult259_g430 );
			#endif
			c.rgb = staticSwitch603.rgb;
			c.a = 1;
			clip( temp_output_892_0.r - _Cutoff );
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
2028;7;1706;1044;-2839.881;2787.404;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;622;1254.999,-848.2166;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;976;343.4865,-2188.488;Inherit;False;Constant;_Vector0;Vector 0;22;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;648;1492.13,-850.6707;Float;False;Standerd_UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;940;2396.893,-3250.76;Inherit;False;963.9407;419.7732;Comment;6;942;898;959;957;897;958;マルチマスクマップ（R.メッシュマスク　G.スペキュラマスク　B.描き影マスク）;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;183;579.3052,-2327.123;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;897;2507.1,-2943.689;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;958;2490.535,-3139.736;Inherit;True;Property;_Tex_MultiMask;Tex_MultiMask;38;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;844.7773,-2308.096;Float;False;Wnorldm;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;1274.619,-1914.834;Inherit;False;258;Wnorldm;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;957;2742.579,-3142.186;Inherit;True;Property;_TextureSample3;Texture Sample 3;23;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewMatrixNode;272;1385.602,-2007.157;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.RangedFloatNode;282;1707.475,-1725.948;Float;False;Constant;_Float15;Float 15;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;959;3080.097,-3007.265;Inherit;False;Multi_TexShadow;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;1540.963,-1936.805;Inherit;True;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;1816.325,-1895.658;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;885;1562.174,-1251.293;Inherit;False;Constant;_Float22;Float 22;45;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;961;1456.421,-1422.831;Inherit;False;959;Multi_TexShadow;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;519;1523.1,-1329.025;Inherit;False;258;Wnorldm;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;276;1968.62,-1846.911;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;751;1748.593,-1357.362;Inherit;False;CH_Light_Shadow_Sepa1;18;;348;666f4a460cae7a5459e5c899bc7bed4f;0;3;374;FLOAT;1;False;363;FLOAT3;0,0,0;False;361;FLOAT;0;False;1;FLOAT;385
Node;AmplifyShaderEditor.TexturePropertyNode;488;2164.888,-2399.424;Float;True;Property;_Tex_Base;Tex_Base;37;0;Create;True;0;0;False;0;None;a3ef2e18f4a1e8c4b94cf10958c03c08;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;675;2154.702,-1354.601;Float;False;Light_Shadow_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;278;2213.436,-1841.254;Float;False;ViewNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;649;2172.763,-2168.723;Inherit;False;648;Standerd_UV;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;696;3691.846,-2481.591;Float;False;Property;_MatCap_Shadowside_Opacity;MatCap_Shadowside_Opacity;10;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;283;1249.147,-2292.308;Inherit;False;278;ViewNormal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;279;1252.907,-2536.357;Float;True;Property;_Tex_MatCap;Tex_MatCap;39;0;Create;True;0;0;False;0;None;2ed438bce1fdc30449f85baf9f25e737;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;410;2615.976,-2178.449;Float;False;Property;_MainColor;++MainColor++++++;7;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;414;2584.27,-1998.999;Float;False;Property;_BlendMainColor;BlendMainColor;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;694;3728.653,-2401.2;Inherit;False;675;Light_Shadow_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;487;2424.643,-2402.126;Inherit;True;Property;_TextureSample0;Texture Sample 0;19;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;903;3543.918,-3040.178;Float;False;Property;_Mesh_Specular_Color;Mesh_Specular_Color;12;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;678;3524.476,-3218.06;Float;False;Property;_Specular_Color;++Specular_Color++++;11;0;Create;True;0;0;False;0;0,0,0,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;898;3074.787,-3172.758;Inherit;False;Mesh_Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;676;3762.38,-2326.845;Float;False;Constant;_Color0;Color 0;32;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;699;3992.741,-2396.943;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;881;3795.662,-2162.237;Inherit;False;Constant;_Vector1;Vector 1;51;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;280;1511.718,-2475.703;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;413;2887.936,-2195.376;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;912;3761.198,-2020.513;Inherit;False;Constant;_Color2;Color 2;25;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;697;3989.492,-2477.21;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;924;3801.413,-3004.487;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;928;3868.12,-3082.318;Inherit;False;Constant;_Float0;Float 0;24;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;281;1824.518,-2487.169;Float;False;Matcap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;626;4039.549,-2212.039;Inherit;False;Blinn-Phong Light;14;;373;cf814dba44d007a4e958d2ddd5813da6;0;3;42;COLOR;0,0,0,0;False;52;FLOAT3;0,0,0;False;43;COLOR;0,0,0,0;False;2;COLOR;0;FLOAT;57
Node;AmplifyShaderEditor.GetLocalVarNode;905;4137.438,-3079.678;Inherit;False;898;Mesh_Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;698;4144.451,-2476.898;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;528;3054.244,-2215.817;Float;False;Mtex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;683;4031.082,-2297.627;Float;False;Property;_Specular_Int;Specular_Int;13;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;923;3800.656,-3222.903;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;963;1627.776,-1102.778;Inherit;False;Property;_Shadow_Opacity;Shadow_Opacity;36;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;284;4177.8,-2764.822;Inherit;True;281;Matcap;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;925;4420.116,-2996.524;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;700;4286.478,-2477.21;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;518;1934.164,-1243.194;Inherit;False;528;Mtex;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.HSVToRGBNode;927;4030.973,-3003.022;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;964;1900.529,-1101.373;Inherit;False;Shadow_opa;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;938;1915.491,-1172.145;Inherit;False;898;Mesh_Mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;680;4336.182,-2275.134;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;490;4130.186,-2575.539;Float;False;Property;_Matcap_Opacity;++Matcap_Opacity+++++++;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;926;4027.086,-3224.076;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;948;4603.938,-3062.434;Inherit;False;305;141;Comment;1;935;合成メッシュスペキュラValue;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;904;4347.671,-3231.794;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;942;3073.62,-3096.693;Inherit;False;Specular_Maskmap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;695;4487.852,-2729.517;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;753;4493.236,-2277.041;Float;False;Specular_Map;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;962;2149.199,-1272.052;Inherit;False;CH_Light_Shadow_Sepa1_5_Nphair;21;;395;f4350e12bf590c14ea9f7916ab0c1516;0;4;348;COLOR;0,0,0,0;False;409;FLOAT;0;False;400;FLOAT;0;False;404;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;935;4615.029,-3002.542;Inherit;False;Mesh_Spec_Value;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;947;4581.938,-3287.434;Inherit;False;305;141;Comment;1;933;合成メッシュスペキュラカラー;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;936;4861.729,-2470.678;Inherit;False;935;Mesh_Spec_Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;943;4864.225,-2543.455;Inherit;False;942;Specular_Maskmap;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;954;4739.49,-2434.161;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;933;4599.082,-3235.275;Inherit;False;Mesh_Spec_Color;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;521;2572.854,-1238.652;Float;True;Light_Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;934;4876.921,-2743.855;Inherit;False;933;Mesh_Spec_Color;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;953;5110.069,-2477.791;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;524;5049.292,-2816.082;Inherit;False;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;489;5276.229,-2762.345;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;863;5431.612,-2765.518;Inherit;False;Mtex_by_Rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;864;5283.417,-2167.43;Inherit;False;863;Mtex_by_Rim;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;967;5250.635,-2227.733;Inherit;False;675;Light_Shadow_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;966;5283.441,-2019.423;Inherit;False;964;Shadow_opa;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;965;5280.813,-2092.086;Inherit;False;521;Light_Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;593;2755.858,-2307.409;Float;False;Mtex_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;968;5515.792,-2101.201;Inherit;False;CH_Light_Shadow_Sepa2_0;-1;;426;5a94b50823288e143a8cd2c1c330b6a5;0;3;385;FLOAT;0;False;348;COLOR;0,0,0,0;False;391;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;969;5510.292,-2223.488;Inherit;False;CH_Light_Shadow_Sepa2_0;-1;;430;5a94b50823288e143a8cd2c1c330b6a5;0;3;385;FLOAT;0;False;348;COLOR;0,0,0,0;False;391;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;890;5581.27,-2516.472;Inherit;False;CH_change_eff;40;;434;0abdb2aaeb62e3645a16d32e261e24b9;0;0;2;COLOR;33;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;891;5841.792,-2531.91;Inherit;False;593;Mtex_Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;597;5943.997,-1768.617;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;893;5984.447,-2250.213;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;977;5966.763,-1841.327;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;604;5953.843,-2153.65;Float;False;Constant;_Check_Color;Check_Color;30;0;Create;True;0;0;False;0;0,1,0.129231,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;892;6038.42,-2512.732;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;883;1591.688,-1600.552;Inherit;False;Constant;_Float21;Float 21;45;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;952;5115.596,-2680.528;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;951;4954.856,-2661.811;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;974;2180.272,-1025.369;Inherit;False;CH_Light_Shadow_Sepa2_Nphair;28;;435;2f79723b56b5eee4493e6973646b4c23;0;3;348;COLOR;0,0,0,0;False;385;FLOAT;0;False;400;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;603;6195.108,-2248.967;Float;False;Property;_Material_Check;Material_Check;6;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;979;6175.153,-1848.96;Inherit;False;CH_Outline2_V2;0;;439;196483622a3b09f4988bb1e9e142f59f;0;3;98;FLOAT;0;False;99;FLOAT4;0,0,0,0;False;100;FLOAT3;0,0,0;False;1;FLOAT3;29
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6552.313,-2473.15;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Girls/Chara/CH_NP_Hair2_V3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;53;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;648;0;622;0
WireConnection;183;0;976;0
WireConnection;258;0;183;0
WireConnection;957;0;958;0
WireConnection;957;1;897;0
WireConnection;959;0;957;3
WireConnection;274;0;272;0
WireConnection;274;1;346;0
WireConnection;275;0;274;0
WireConnection;275;1;282;0
WireConnection;276;0;275;0
WireConnection;276;1;282;0
WireConnection;751;374;961;0
WireConnection;751;363;519;0
WireConnection;751;361;885;0
WireConnection;675;0;751;385
WireConnection;278;0;276;0
WireConnection;487;0;488;0
WireConnection;487;1;649;0
WireConnection;898;0;957;1
WireConnection;699;0;694;0
WireConnection;280;0;279;0
WireConnection;280;1;283;0
WireConnection;413;0;487;0
WireConnection;413;1;410;0
WireConnection;413;2;414;0
WireConnection;697;0;696;0
WireConnection;924;0;903;0
WireConnection;281;0;280;0
WireConnection;626;42;676;0
WireConnection;626;52;881;0
WireConnection;626;43;912;0
WireConnection;698;0;697;0
WireConnection;698;1;699;0
WireConnection;528;0;413;0
WireConnection;923;0;678;0
WireConnection;925;0;923;3
WireConnection;925;1;924;3
WireConnection;925;2;905;0
WireConnection;700;0;698;0
WireConnection;927;0;924;1
WireConnection;927;1;924;2
WireConnection;927;2;928;0
WireConnection;964;0;963;0
WireConnection;680;0;683;0
WireConnection;680;1;626;0
WireConnection;680;2;626;0
WireConnection;926;0;923;1
WireConnection;926;1;923;2
WireConnection;926;2;928;0
WireConnection;904;0;926;0
WireConnection;904;1;927;0
WireConnection;904;2;905;0
WireConnection;942;0;957;2
WireConnection;695;0;284;0
WireConnection;695;1;490;0
WireConnection;695;2;700;0
WireConnection;753;0;680;0
WireConnection;962;348;518;0
WireConnection;962;409;751;385
WireConnection;962;400;938;0
WireConnection;962;404;964;0
WireConnection;935;0;925;0
WireConnection;954;0;695;0
WireConnection;954;1;753;0
WireConnection;933;0;904;0
WireConnection;521;0;962;0
WireConnection;953;0;943;0
WireConnection;953;1;936;0
WireConnection;953;2;954;0
WireConnection;489;0;524;0
WireConnection;489;1;934;0
WireConnection;489;2;953;0
WireConnection;863;0;489;0
WireConnection;593;0;487;4
WireConnection;968;385;967;0
WireConnection;968;348;965;0
WireConnection;968;391;966;0
WireConnection;969;385;967;0
WireConnection;969;348;864;0
WireConnection;969;391;966;0
WireConnection;893;0;890;33
WireConnection;893;1;969;0
WireConnection;977;0;968;0
WireConnection;892;0;891;0
WireConnection;892;1;890;0
WireConnection;952;0;934;0
WireConnection;952;1;951;1
WireConnection;974;348;518;0
WireConnection;974;385;751;385
WireConnection;974;400;938;0
WireConnection;603;1;893;0
WireConnection;603;0;604;0
WireConnection;979;98;892;0
WireConnection;979;99;977;0
WireConnection;979;100;597;0
WireConnection;0;10;892;0
WireConnection;0;13;603;0
WireConnection;0;11;979;29
ASEEND*/
//CHKSM=3242D4F9B5FC7E300ABC8290BD209B8C53BEC7CC