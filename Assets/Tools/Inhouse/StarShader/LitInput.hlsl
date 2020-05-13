#ifndef STAR_UNIVERSAL_LIT_INPUT_INCLUDED
#define STAR_UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _SpecColor;
half4 _EmissionColor;
half _Cutoff;
half _BumpScale;
half _OcclusionStrength;
half _MetallicMapScale;
half _RoughnessMapScale;
half _Metallic;
half _Roughness;
CBUFFER_END

#ifdef _MRAMAP
    TEXTURE2D(_MRAMap);             SAMPLER(sampler_MRAMap);
#endif

half3 SampleMRA(half2 uv)
{
#if _MRAMAP
    half3 mra = SAMPLE_TEXTURE2D(_MRAMap, sampler_MRAMap, uv).rgb;
    mra.r *= _MetallicMapScale;
    mra.g = 1.0 - _RoughnessMapScale * mra.g;
#else
    half3 mra = half3(_Metallic, 1.0 - _Roughness, 1);
#endif
    return mra;
}

inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

    half3 mra = SampleMRA(uv);
    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;

    outSurfaceData.metallic = mra.r;
    outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);

    outSurfaceData.smoothness = mra.g;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    outSurfaceData.occlusion = LerpWhiteTo(mra.b, _OcclusionStrength);
    outSurfaceData.emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
}

#endif // STAR_UNIVERSAL_INPUT_SURFACE_PBR_INCLUDED
