// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Girls/Chara/CH_NP_Eyebrow_V3"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Toggle(_MATERIAL_CHECK_ON)] _Material_Check("Material_Check", Float) = 0
		_Eyebrow_Offset("Eyebrow_Offset", Float) = 0.1
		_MainColor("++MainColor++++++", Color) = (1,1,1,1)
		_BlendMainColor("BlendMainColor", Range( 0 , 1)) = 0
		_Tex_Base("Tex_Base", 2D) = "white" {}
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
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _MATERIAL_CHECK_ON
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
			float2 uv_texcoord;
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

		uniform float _Eyebrow_Offset;
		uniform float _c_Level;
		uniform float _c_Smooth;
		uniform float _c_reverse;
		uniform sampler2D _c_Gradation;
		uniform float _Bottom;
		uniform float _Top;
		uniform sampler2D _c_DotTex1;
		uniform sampler2D _Tex_Base;
		uniform float4 _MainColor;
		uniform float _BlendMainColor;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 worldToObjDir761 = mul( unity_WorldToObject, float4( ( _Eyebrow_Offset * ase_worldViewDir ), 0 ) ).xyz;
			v.vertex.xyz += worldToObjDir761;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float temp_output_48_0_g355 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_0 = (( _c_Level - temp_output_48_0_g355 )).xxxx;
			float4 temp_cast_1 = (( _c_Level + temp_output_48_0_g355 )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g355 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g355 = tex2D( _c_Gradation, appendResult39_g355 );
			float4 smoothstepResult54_g355 = smoothstep( temp_cast_0 , temp_cast_1 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g355 ) ):( tex2DNode41_g355 )) ) ));
			float4 blendOpSrc59_g355 = smoothstepResult54_g355;
			float4 blendOpDest59_g355 = ( 1.0 - smoothstepResult54_g355 );
			float4 temp_output_59_0_g355 = ( saturate( ( blendOpSrc59_g355 * blendOpDest59_g355 ) ));
			float2 temp_output_53_0_g355 = ( appendResult39_g355 * float2( 5,5 ) );
			float2 panner57_g355 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g355);
			float mulTime55_g355 = _Time.y * 0.5;
			float2 panner58_g355 = ( mulTime55_g355 * float2( 0,0 ) + temp_output_53_0_g355);
			float4 blendOpSrc64_g355 = ( temp_output_59_0_g355 + temp_output_59_0_g355 );
			float4 blendOpDest64_g355 = ( tex2D( _c_DotTex1, panner57_g355 ) * tex2D( _c_DotTex1, panner58_g355 ) );
			float4 temp_output_64_0_g355 = ( saturate( ( blendOpSrc64_g355 * blendOpDest64_g355 ) ));
			float4 clampResult69_g355 = clamp( ( smoothstepResult54_g355 + temp_output_64_0_g355 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 lerpResult413 = lerp( ( float4( ase_lightColor.rgb , 0.0 ) * tex2D( _Tex_Base, i.uv_texcoord ) ) , _MainColor , _BlendMainColor);
			float4 color604 = IsGammaSpace() ? float4(0,1,0.129231,1) : float4(0,1,0.01517276,1);
			#ifdef _MATERIAL_CHECK_ON
				float4 staticSwitch603 = color604;
			#else
				float4 staticSwitch603 = ( temp_output_64_0_g355 + lerpResult413 );
			#endif
			c.rgb = staticSwitch603.rgb;
			c.a = 1;
			clip( clampResult69_g355.r - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17400
2028;7;1706;1044;-4134.002;3305.01;1.996825;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;622;5741.194,-2321.251;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;488;5750.279,-2556.552;Float;True;Property;_Tex_Base;Tex_Base;5;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;487;6009.924,-2556.175;Inherit;True;Property;_TextureSample0;Texture Sample 0;19;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;766;6103.851,-2692.286;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;757;6212.404,-1964.133;Inherit;False;613.7992;336.9407;eye blow;4;761;760;758;759;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;767;6311.886,-2622.276;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;414;6203.781,-2158.005;Float;False;Property;_BlendMainColor;BlendMainColor;4;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;410;6235.487,-2337.455;Float;False;Property;_MainColor;++MainColor++++++;3;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;758;6227.31,-1902.159;Float;False;Property;_Eyebrow_Offset;Eyebrow_Offset;2;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;762;6483.988,-2477.017;Inherit;False;CH_change_eff;6;;355;0abdb2aaeb62e3645a16d32e261e24b9;0;0;2;COLOR;33;COLOR;0
Node;AmplifyShaderEditor.LerpOp;413;6507.447,-2354.382;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;759;6239.221,-1783.006;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;760;6423.386,-1870.817;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;765;6699.813,-2369.29;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;604;6628.721,-2224.172;Float;False;Constant;_Check_Color;Check_Color;30;0;Create;True;0;0;False;0;0,1,0.129231,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformDirectionNode;761;6563.618,-1881.962;Inherit;False;World;Object;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;603;6843.594,-2374.921;Float;False;Property;_Material_Check;Material_Check;1;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;7162.129,-2638.281;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Girls/Chara/CH_NP_Eyebrow_V3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;487;0;488;0
WireConnection;487;1;622;0
WireConnection;767;0;766;1
WireConnection;767;1;487;0
WireConnection;413;0;767;0
WireConnection;413;1;410;0
WireConnection;413;2;414;0
WireConnection;760;0;758;0
WireConnection;760;1;759;0
WireConnection;765;0;762;33
WireConnection;765;1;413;0
WireConnection;761;0;760;0
WireConnection;603;1;765;0
WireConnection;603;0;604;0
WireConnection;0;10;762;0
WireConnection;0;13;603;0
WireConnection;0;11;761;0
ASEEND*/
//CHKSM=6EBB60A4FF9B6903B5D0A3C7DF4599FEDBF286B5