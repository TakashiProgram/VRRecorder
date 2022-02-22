// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Girls/Chara/CH_NP_Eye_V3"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Toggle(_MATERIAL_CHECK_ON)] _Material_Check("Material_Check", Float) = 0
		_Height_Int("Height_Int", Range( 0 , 0.2)) = 0.03
		_Leapp("++Leapp++++++", Range( 0 , 1)) = 0.6
		_UV_U("UV_U", Range( -1 , 1)) = 0.43
		_UV_V("UV_V", Range( -1 , 1)) = -0.23
		_MatCap_int_Mlt("++MatCap_int_Mlt++", Range( 0 , 1)) = 0.4
		_Normal_Scale("++Normal_Scale+++++", Range( 0 , 5)) = 2
		_FallOff_Int("++FallOff_Int+++++", Range( 0 , 5)) = 0.5
		_Emi_Int("++Emi_Int++", Range( 0 , 5)) = 0
		_Hilight_Yure_Speed("++Hilight_Yure_Speed+++++", Range( 0 , 100)) = 55
		_Hilight_Yure_Int("Hilight_Yure_Int", Range( 0 , 0.5)) = 0.01
		_Tex_BaseMap("Tex_BaseMap", 2D) = "white" {}
		_Tex_Hilight("Tex_Hilight", 2D) = "black" {}
		_Tex_Underhilight("Tex_Underhilight", 2D) = "black" {}
		_Tex_MatCap("Tex_MatCap", 2D) = "white" {}
		_Tex_Nrm("Tex_Nrm", 2D) = "bump" {}
		_Tex_Heightmap("Tex_Heightmap", 2D) = "white" {}
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
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.5
		#pragma shader_feature _MATERIAL_CHECK_ON
		#pragma surface surf StandardCustomLighting keepalpha noshadow 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
			float3 viewDir;
			float4 screenPos;
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

		uniform float _Emi_Int;
		uniform sampler2D _Tex_Hilight;
		uniform float _Normal_Scale;
		uniform sampler2D _Tex_Nrm;
		uniform float4 _Tex_Nrm_ST;
		uniform float _UV_U;
		uniform float _Hilight_Yure_Speed;
		uniform float _Hilight_Yure_Int;
		uniform float _UV_V;
		uniform float _Leapp;
		uniform sampler2D _Tex_Underhilight;
		uniform float _FallOff_Int;
		uniform sampler2D _Tex_BaseMap;
		uniform sampler2D _Tex_Heightmap;
		uniform float4 _Tex_Heightmap_ST;
		uniform float _Height_Int;
		uniform sampler2D _Tex_MatCap;
		uniform float _MatCap_int_Mlt;
		uniform float _c_Level;
		uniform float _c_Smooth;
		uniform float _c_reverse;
		uniform sampler2D _c_Gradation;
		uniform float _Bottom;
		uniform float _Top;
		uniform sampler2D _c_DotTex1;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
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

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float temp_output_48_0_g358 = ( sin( ( _c_Level * 3.14 ) ) * _c_Smooth );
			float4 temp_cast_11 = (( _c_Level - temp_output_48_0_g358 )).xxxx;
			float4 temp_cast_12 = (( _c_Level + temp_output_48_0_g358 )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult39_g358 = (float2((0.0 + (ase_worldPos.y - _Bottom) * (1.0 - 0.0) / (_Top - _Bottom)) , ase_screenPosNorm.x));
			float4 tex2DNode41_g358 = tex2D( _c_Gradation, appendResult39_g358 );
			float4 smoothstepResult54_g358 = smoothstep( temp_cast_11 , temp_cast_12 , (( _c_Level == 1.0 ) ? float4( 0,0,0,0 ) :  (( _c_Level == 0.0 ) ? float4( 1,1,1,0 ) :  (( _c_reverse )?( ( 1.0 - tex2DNode41_g358 ) ):( tex2DNode41_g358 )) ) ));
			float4 blendOpSrc59_g358 = smoothstepResult54_g358;
			float4 blendOpDest59_g358 = ( 1.0 - smoothstepResult54_g358 );
			float4 temp_output_59_0_g358 = ( saturate( ( blendOpSrc59_g358 * blendOpDest59_g358 ) ));
			float2 temp_output_53_0_g358 = ( appendResult39_g358 * float2( 5,5 ) );
			float2 panner57_g358 = ( _Time.y * float2( -0.1,0 ) + temp_output_53_0_g358);
			float mulTime55_g358 = _Time.y * 0.5;
			float2 panner58_g358 = ( mulTime55_g358 * float2( 0,0 ) + temp_output_53_0_g358);
			float4 blendOpSrc64_g358 = ( temp_output_59_0_g358 + temp_output_59_0_g358 );
			float4 blendOpDest64_g358 = ( tex2D( _c_DotTex1, panner57_g358 ) * tex2D( _c_DotTex1, panner58_g358 ) );
			float4 temp_output_64_0_g358 = ( saturate( ( blendOpSrc64_g358 * blendOpDest64_g358 ) ));
			float4 clampResult69_g358 = clamp( ( smoothstepResult54_g358 + temp_output_64_0_g358 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float2 temp_cast_15 = (1.0).xx;
			float2 uv_TexCoord604 = i.uv_texcoord * temp_cast_15;
			float2 uv_Tex_Heightmap = i.uv_texcoord * _Tex_Heightmap_ST.xy + _Tex_Heightmap_ST.zw;
			float4 tex2DNode592 = tex2D( _Tex_Heightmap, uv_Tex_Heightmap );
			float2 Offset596 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + uv_TexCoord604;
			float2 Offset610 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset596;
			float2 Offset611 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset610;
			float2 Offset612 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset611;
			float2 uv_Tex_Nrm = i.uv_texcoord * _Tex_Nrm_ST.xy + _Tex_Nrm_ST.zw;
			float3 Wnorldm_NM688 = (WorldNormalVector( i , UnpackScaleNormal( tex2D( _Tex_Nrm, uv_Tex_Nrm ), _Normal_Scale ) ));
			float3 ViewNormal714 = ( ( mul( UNITY_MATRIX_V, float4( Wnorldm_NM688 , 0.0 ) ).xyz * 0.5 ) + 0.5 );
			float4 Matcap718 = tex2D( _Tex_MatCap, ViewNormal714.xy );
			float4 clampResult800 = clamp( ( Matcap718 + _MatCap_int_Mlt ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float mulTime820 = _Time.y * _Hilight_Yure_Speed;
			float2 temp_cast_19 = (mulTime820).xx;
			float simplePerlin2D833 = snoise( temp_cast_19 );
			float temp_output_829_0 = ( mulTime820 * 0.5 );
			float2 temp_cast_20 = (temp_output_829_0).xx;
			float simplePerlin2D837 = snoise( temp_cast_20 );
			float4 appendResult738 = (float4(( (ViewNormal714).x + _UV_U + ( (simplePerlin2D833*2.0 + -1.0) * _Hilight_Yure_Int ) ) , ( (ViewNormal714).y + _UV_V + ( ( (simplePerlin2D837*2.0 + -1.0) * _Hilight_Yure_Int ) * 0.5 ) ) , 0.0 , 0.0));
			float4 lerpResult728 = lerp( appendResult738 , float4( i.uv_texcoord, 0.0 , 0.0 ) , _Leapp);
			float4 tex2DNode620 = tex2D( _Tex_Hilight, lerpResult728.xy );
			float4 blendOpSrc726 = ( tex2D( _Tex_BaseMap, Offset612 ) * clampResult800 );
			float4 blendOpDest726 = tex2DNode620;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float dotResult757 = dot( ase_worldViewDir , ase_worldNormal );
			float FallOff762 = ( 1.0 - dotResult757 );
			float4 temp_output_763_0 = ( tex2D( _Tex_Underhilight, ViewNormal714.xy ) * pow( ( 1.0 - FallOff762 ) , _FallOff_Int ) );
			float4 blendOpSrc751 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc726 ) * ( 1.0 - blendOpDest726 ) ) ));
			float4 blendOpDest751 = temp_output_763_0;
			float4 temp_output_751_0 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc751 ) * ( 1.0 - blendOpDest751 ) ) ));
			float4 color854 = IsGammaSpace() ? float4(0,1,0.129231,1) : float4(0,1,0.01517276,1);
			#ifdef _MATERIAL_CHECK_ON
				float4 staticSwitch855 = color854;
			#else
				float4 staticSwitch855 = ( float4( ase_lightColor.rgb , 0.0 ) * temp_output_751_0 );
			#endif
			c.rgb = ( staticSwitch855 + temp_output_64_0_g358 ).rgb;
			c.a = 1;
			clip( clampResult69_g358.r - _Cutoff );
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
			float2 uv_Tex_Nrm = i.uv_texcoord * _Tex_Nrm_ST.xy + _Tex_Nrm_ST.zw;
			float3 Wnorldm_NM688 = (WorldNormalVector( i , UnpackScaleNormal( tex2D( _Tex_Nrm, uv_Tex_Nrm ), _Normal_Scale ) ));
			float3 ViewNormal714 = ( ( mul( UNITY_MATRIX_V, float4( Wnorldm_NM688 , 0.0 ) ).xyz * 0.5 ) + 0.5 );
			float mulTime820 = _Time.y * _Hilight_Yure_Speed;
			float2 temp_cast_2 = (mulTime820).xx;
			float simplePerlin2D833 = snoise( temp_cast_2 );
			float temp_output_829_0 = ( mulTime820 * 0.5 );
			float2 temp_cast_3 = (temp_output_829_0).xx;
			float simplePerlin2D837 = snoise( temp_cast_3 );
			float4 appendResult738 = (float4(( (ViewNormal714).x + _UV_U + ( (simplePerlin2D833*2.0 + -1.0) * _Hilight_Yure_Int ) ) , ( (ViewNormal714).y + _UV_V + ( ( (simplePerlin2D837*2.0 + -1.0) * _Hilight_Yure_Int ) * 0.5 ) ) , 0.0 , 0.0));
			float4 lerpResult728 = lerp( appendResult738 , float4( i.uv_texcoord, 0.0 , 0.0 ) , _Leapp);
			float4 tex2DNode620 = tex2D( _Tex_Hilight, lerpResult728.xy );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float dotResult757 = dot( ase_worldViewDir , ase_worldNormal );
			float FallOff762 = ( 1.0 - dotResult757 );
			float4 temp_output_763_0 = ( tex2D( _Tex_Underhilight, ViewNormal714.xy ) * pow( ( 1.0 - FallOff762 ) , _FallOff_Int ) );
			float2 temp_cast_7 = (1.0).xx;
			float2 uv_TexCoord604 = i.uv_texcoord * temp_cast_7;
			float2 uv_Tex_Heightmap = i.uv_texcoord * _Tex_Heightmap_ST.xy + _Tex_Heightmap_ST.zw;
			float4 tex2DNode592 = tex2D( _Tex_Heightmap, uv_Tex_Heightmap );
			float2 Offset596 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + uv_TexCoord604;
			float2 Offset610 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset596;
			float2 Offset611 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset610;
			float2 Offset612 = ( ( tex2DNode592.r - 1 ) * ( i.viewDir.xy / i.viewDir.z ) * _Height_Int ) + Offset611;
			float4 Matcap718 = tex2D( _Tex_MatCap, ViewNormal714.xy );
			float4 clampResult800 = clamp( ( Matcap718 + _MatCap_int_Mlt ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 blendOpSrc726 = ( tex2D( _Tex_BaseMap, Offset612 ) * clampResult800 );
			float4 blendOpDest726 = tex2DNode620;
			float4 blendOpSrc751 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc726 ) * ( 1.0 - blendOpDest726 ) ) ));
			float4 blendOpDest751 = temp_output_763_0;
			float4 temp_output_751_0 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc751 ) * ( 1.0 - blendOpDest751 ) ) ));
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 hsvTorgb808 = RGBToHSV( ase_lightColor.rgb );
			o.Emission = ( ( ( _Emi_Int * ( tex2DNode620 + temp_output_763_0 ) ) + ( 0.1 * temp_output_751_0 ) ) * pow( hsvTorgb808.z , 4.0 ) ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
352;12;1561;1119;-3613.028;4697.681;2.684096;True;True
Node;AmplifyShaderEditor.CommentaryNode;684;3406.21,-3257.708;Inherit;False;1645.893;680.9099;WorldNormal_with_NM;4;679;687;688;685;WorldNormal_with_NM;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;685;3916.885,-2978.61;Float;False;Property;_Normal_Scale;++Normal_Scale+++++;7;0;Create;True;0;0;False;0;2;1.074;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;679;4227.328,-3061.851;Inherit;True;Property;_Tex_Nrm;Tex_Nrm;16;0;Create;True;0;0;False;0;-1;00c6bd74b483903409b8c8a1b7de5d2c;00c6bd74b483903409b8c8a1b7de5d2c;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;687;4555.413,-3055.013;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;707;5237.792,-3243.963;Inherit;False;1188.92;507.3546;ViewNormal_UV;8;714;713;712;711;710;709;708;722;ViewNormal_UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;688;4796.535,-3062.249;Float;False;Wnorldm_NM;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;818;6653.856,-3610.837;Float;False;Property;_Hilight_Yure_Speed;++Hilight_Yure_Speed+++++;10;0;Create;True;0;0;False;0;55;55;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewMatrixNode;709;5386.34,-3111.88;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.GetLocalVarNode;708;5275.357,-3019.556;Inherit;False;688;Wnorldm_NM;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;710;5708.213,-2830.671;Float;False;Constant;_Float15;Float 15;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;711;5541.702,-3041.528;Inherit;True;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;820;6973.856,-3633.837;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;830;6962.856,-3549.837;Float;False;Constant;_Float2;Float 2;17;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;829;7132.955,-3570.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;712;5817.063,-3000.38;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;837;7002.069,-4011.462;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;840;7180.025,-4115.712;Float;False;Constant;_Float4;Float 4;24;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;713;5969.359,-2951.634;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;841;7214.341,-4233.682;Float;False;Constant;_Float5;Float 5;24;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;825;7269.856,-3552.837;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;714;6214.174,-2945.976;Float;False;ViewNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;605;6666.792,-2596.365;Float;False;Constant;_Tiling;Tiling;6;0;Create;True;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;715;4980.16,-3798.844;Inherit;False;1049.579;448.4371;MatCap;4;719;716;717;718;MatCap;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;845;7353.345,-3988.275;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;833;6988.012,-4174.722;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;844;7389.746,-4213.175;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;756;5207.685,-2548.134;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;594;6858.021,-2018.409;Float;False;Property;_Height_Int;Height_Int;2;0;Create;True;0;0;False;0;0.03;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;755;5194.979,-2393.712;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;595;6895.275,-2453.183;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;719;5271.873,-3729.073;Float;True;Property;_Tex_MatCap;Tex_MatCap;15;0;Create;True;0;0;False;0;None;98195b6717a8e3f488c9949e41bc5757;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;592;6795.616,-2279.166;Inherit;True;Property;_Tex_Heightmap;Tex_Heightmap;17;0;Create;True;0;0;False;0;-1;5c0fe9923cb35aa44922042de2278ffd;5c0fe9923cb35aa44922042de2278ffd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchNode;843;7604.299,-3824.219;Inherit;False;0;2;8;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;716;5110.375,-3500.531;Inherit;False;714;ViewNormal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;822;7132.856,-3480.837;Float;False;Property;_Hilight_Yure_Int;Hilight_Yure_Int;11;0;Create;True;0;0;False;0;0.01;0.01;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;824;7261.856,-3627.837;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;604;6944.662,-2613.247;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;826;7437.856,-3545.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;727;6850.698,-3325.262;Inherit;False;714;ViewNormal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;717;5521.391,-3683.285;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchNode;842;7633.374,-3973.069;Inherit;False;0;2;8;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;757;5407.202,-2526.335;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;596;7244.692,-2529.753;Inherit;False;Planar;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;731;7216.521,-3406.267;Inherit;False;True;False;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;831;7593.158,-3524.337;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;758;5531.504,-2524.742;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;732;7220.497,-3205.904;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;610;7243.156,-2366.03;Inherit;False;Planar;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;718;5834.191,-3694.751;Float;False;Matcap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;734;7159.054,-3111.139;Float;False;Property;_UV_V;UV_V;5;0;Create;True;0;0;False;0;-0.23;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;733;7150.74,-3322.102;Float;False;Property;_UV_U;UV_U;4;0;Create;True;0;0;False;0;0.43;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;816;7442.958,-3653.737;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;736;7432.489,-3175.193;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;796;7542.709,-2522.308;Float;False;Property;_MatCap_int_Mlt;++MatCap_int_Mlt++;6;0;Create;True;0;0;False;0;0.4;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;762;5688.102,-2527.835;Float;False;FallOff;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;794;7628.647,-2608.253;Inherit;False;718;Matcap;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;735;7447.277,-3365.537;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;611;7239.056,-2210.934;Inherit;False;Planar;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;738;7586.287,-3273.794;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;798;7820.801,-2607.072;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ParallaxMappingNode;612;7242.272,-2064.862;Inherit;False;Planar;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;764;7777.129,-2237.041;Inherit;False;762;FallOff;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;621;7447.209,-3018.4;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;729;7405.253,-2884.089;Float;False;Property;_Leapp;++Leapp++++++;3;0;Create;True;0;0;False;0;0.6;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;728;7789.529,-3059.623;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;765;7962.427,-2230.582;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;602;7518.852,-2797.547;Inherit;True;Property;_Tex_BaseMap;Tex_BaseMap;12;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;800;7946.486,-2653.935;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;761;7843.945,-2152.027;Float;False;Property;_FallOff_Int;++FallOff_Int+++++;8;0;Create;True;0;0;False;0;0.5;3;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;753;7772.425,-2365.277;Inherit;False;714;ViewNormal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;620;7993.448,-3080.044;Inherit;True;Property;_Tex_Hilight;Tex_Hilight;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;797;8085.299,-2809.354;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;760;8132.91,-2234.603;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;752;8005.497,-2505.929;Inherit;True;Property;_Tex_Underhilight;Tex_Underhilight;14;0;Create;True;0;0;False;0;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;763;8411.56,-2706.088;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;726;8419.254,-2929.35;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;771;8592.432,-3205.391;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;774;8697.239,-3122.056;Float;False;Constant;_Float1;Float 1;18;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;751;8656.636,-2936.605;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;768;8443.093,-3341.347;Float;False;Property;_Emi_Int;++Emi_Int++;9;0;Create;True;0;0;False;0;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;801;8898.299,-3035.676;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;766;8747.205,-3219.982;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RGBToHSVNode;808;9200.532,-3102.822;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;772;8862.724,-3116.749;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;802;9099.934,-2951.09;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;854;9035.039,-2799.448;Float;False;Constant;_Check_Color1;Check_Color;30;0;Create;True;0;0;False;0;0,1,0.129231,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;812;9414.518,-3103.414;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;850;9425.237,-2683.02;Inherit;False;CH_change_eff;18;;358;0abdb2aaeb62e3645a16d32e261e24b9;0;0;2;COLOR;33;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;855;9242.999,-2946.448;Float;False;Property;_Material_Check;Material_Check;1;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;773;9006.309,-3189.43;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;810;9557.791,-3162.007;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;722;5294.642,-2873.894;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;853;9547.301,-2937.819;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;9703.547,-3208.231;Float;False;True;-1;3;ASEMaterialInspector;0;0;CustomLighting;Girls/Chara/CH_NP_Eye_V3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;679;5;685;0
WireConnection;687;0;679;0
WireConnection;688;0;687;0
WireConnection;711;0;709;0
WireConnection;711;1;708;0
WireConnection;820;0;818;0
WireConnection;829;0;820;0
WireConnection;829;1;830;0
WireConnection;712;0;711;0
WireConnection;712;1;710;0
WireConnection;837;0;829;0
WireConnection;713;0;712;0
WireConnection;713;1;710;0
WireConnection;825;0;829;0
WireConnection;714;0;713;0
WireConnection;845;0;837;0
WireConnection;845;1;841;0
WireConnection;845;2;840;0
WireConnection;833;0;820;0
WireConnection;844;0;833;0
WireConnection;844;1;841;0
WireConnection;844;2;840;0
WireConnection;843;0;845;0
WireConnection;843;1;825;0
WireConnection;824;0;820;0
WireConnection;604;0;605;0
WireConnection;826;0;843;0
WireConnection;826;1;822;0
WireConnection;717;0;719;0
WireConnection;717;1;716;0
WireConnection;842;0;844;0
WireConnection;842;1;824;0
WireConnection;757;0;756;0
WireConnection;757;1;755;0
WireConnection;596;0;604;0
WireConnection;596;1;592;1
WireConnection;596;2;594;0
WireConnection;596;3;595;0
WireConnection;731;0;727;0
WireConnection;831;0;826;0
WireConnection;758;0;757;0
WireConnection;732;0;727;0
WireConnection;610;0;596;0
WireConnection;610;1;592;1
WireConnection;610;2;594;0
WireConnection;610;3;595;0
WireConnection;718;0;717;0
WireConnection;816;0;842;0
WireConnection;816;1;822;0
WireConnection;736;0;732;0
WireConnection;736;1;734;0
WireConnection;736;2;831;0
WireConnection;762;0;758;0
WireConnection;735;0;731;0
WireConnection;735;1;733;0
WireConnection;735;2;816;0
WireConnection;611;0;610;0
WireConnection;611;1;592;1
WireConnection;611;2;594;0
WireConnection;611;3;595;0
WireConnection;738;0;735;0
WireConnection;738;1;736;0
WireConnection;798;0;794;0
WireConnection;798;1;796;0
WireConnection;612;0;611;0
WireConnection;612;1;592;1
WireConnection;612;2;594;0
WireConnection;612;3;595;0
WireConnection;728;0;738;0
WireConnection;728;1;621;0
WireConnection;728;2;729;0
WireConnection;765;0;764;0
WireConnection;602;1;612;0
WireConnection;800;0;798;0
WireConnection;620;1;728;0
WireConnection;797;0;602;0
WireConnection;797;1;800;0
WireConnection;760;0;765;0
WireConnection;760;1;761;0
WireConnection;752;1;753;0
WireConnection;763;0;752;0
WireConnection;763;1;760;0
WireConnection;726;0;797;0
WireConnection;726;1;620;0
WireConnection;771;0;620;0
WireConnection;771;1;763;0
WireConnection;751;0;726;0
WireConnection;751;1;763;0
WireConnection;766;0;768;0
WireConnection;766;1;771;0
WireConnection;808;0;801;1
WireConnection;772;0;774;0
WireConnection;772;1;751;0
WireConnection;802;0;801;1
WireConnection;802;1;751;0
WireConnection;812;0;808;3
WireConnection;855;1;802;0
WireConnection;855;0;854;0
WireConnection;773;0;766;0
WireConnection;773;1;772;0
WireConnection;810;0;773;0
WireConnection;810;1;812;0
WireConnection;853;0;855;0
WireConnection;853;1;850;33
WireConnection;0;2;810;0
WireConnection;0;10;850;0
WireConnection;0;13;853;0
ASEEND*/
//CHKSM=1D52BEA47FD6D712A7B6899E69F55452783B9C2F