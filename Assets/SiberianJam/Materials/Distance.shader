Shader "Custom/DistanceFadeLitURP"
{
    Properties
    {
         // --- Albedo: единственный источник тайлинга/оффсета ---
    [MainTexture] _BaseMap ("Albedo (Base Map) Ч master tiling/offset", 2D) = "white" {}
    [MainColor]   _BaseColor ("Base Color", Color) = (1,1,1,1)
    [Toggle(_ALPHA_FROM_MAP)] _UseMapAlpha ("Use Albedo Texture Alpha", Float) = 0

    // --- Normal ---
    [Toggle(_NORMALMAP)] _UseNormalMap ("Use Normal Map", Float) = 0
    [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
    _BumpScale ("Normal Scale", Range(0, 2)) = 1.0

    // --- Metallic / Smoothness ---
    [Toggle(_METALLICSPECGLOSSMAP)] _UseMetallicMap ("Use Metallic/Smoothness Map", Float) = 0
    [NoScaleOffset] _MetallicGlossMap ("Metallic-Smoothness Map", 2D) = "white" {}
    _Metallic ("Metallic", Range(0, 1)) = 0.0
    _Smoothness ("Smoothness", Range(0, 1)) = 0.5

    // --- Occlusion ---
    [Toggle(_OCCLUSIONMAP)] _UseOcclusionMap ("Use Occlusion Map", Float) = 0
    [NoScaleOffset] _OcclusionMap ("Occlusion Map", 2D) = "white" {}
    _OcclusionStrength ("Occlusion Strength", Range(0, 1)) = 1.0

    // --- Emission ---
    [Toggle(_EMISSION)] _UseEmission ("Use Emission", Float) = 0
    [NoScaleOffset] _EmissionMap ("Emission Map", 2D) = "white" {}
    [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,0)

    // --- Fade ---
    _FadeStart ("Fade Start Distance", Float) = 5.0
    _FadeEnd ("Fade End Distance", Float) = 1.0
    _FadeCurve ("Fade Curve (0=linear, >0=ease)", Float) = 0.5
    }

        SubShader
                        {
                            Tags
                            {
                                "RenderType" = "Transparent"
                                "Queue" = "Transparent"
                                "RenderPipeline" = "UniversalPipeline"
                                "IgnoreProjector" = "True"
                            }

                            Blend SrcAlpha OneMinusSrcAlpha
                            ZWrite Off
                            Cull Back

                            Pass
                            {
                                Name "UniversalForward"
                                Tags { "LightMode" = "UniversalForward" }

                                HLSLPROGRAM
                                #pragma vertex vert
                                #pragma fragment frag

                                #pragma shader_feature_local _ALPHA_FROM_MAP
                                #pragma shader_feature_local _NORMALMAP
                                #pragma shader_feature_local _METALLICSPECGLOSSMAP
                                #pragma shader_feature_local _OCCLUSIONMAP
                                #pragma shader_feature_local _EMISSION

                                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
                                #pragma multi_compile _ _SHADOWS_SOFT
                                #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
                                #pragma multi_compile_fog

                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                                struct Attributes
                                {
                                    float4 positionOS : POSITION;
                                    float3 normalOS   : NORMAL;
                                    float4 tangentOS  : TANGENT;
                                    float2 uv         : TEXCOORD0;
                                };

                                struct Varyings
                                {
                                    float4 positionCS  : SV_POSITION;
                                    // ≈диный UV дл€ всех карт: масштабируетс€ только _BaseMap_ST
                                    float2 uv          : TEXCOORD0;
                                    float3 positionWS  : TEXCOORD1;
                                    float3 normalWS    : TEXCOORD2;
                                    half3 tangentWS    : TEXCOORD3;
                                    half3 bitangentWS  : TEXCOORD4;
                                    half  fogFactor    : TEXCOORD5;
                                };

                                TEXTURE2D(_BaseMap);           SAMPLER(sampler_BaseMap);
                                TEXTURE2D(_BumpMap);           SAMPLER(sampler_BumpMap);
                                TEXTURE2D(_MetallicGlossMap);  SAMPLER(sampler_MetallicGlossMap);
                                TEXTURE2D(_OcclusionMap);      SAMPLER(sampler_OcclusionMap);
                                TEXTURE2D(_EmissionMap);       SAMPLER(sampler_EmissionMap);

                                CBUFFER_START(UnityPerMaterial)
                                    float4 _BaseMap_ST;   // единственный мастер-тайлинг дл€ всех карт
                                    float4 _BaseColor;
                                    float  _BumpScale;
                                    float  _Metallic;
                                    float  _Smoothness;
                                    float  _OcclusionStrength;
                                    float4 _EmissionColor;
                                    float  _FadeStart;
                                    float  _FadeEnd;
                                    float  _FadeCurve;
                                CBUFFER_END

                                Varyings vert(Attributes IN)
                                {
                                    Varyings OUT;

                                    OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                                    OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                                    OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                                    // ќдин общий UV: мастера Ч тайлинг и оффсет Albedo-карты
                                    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                                    OUT.fogFactor = ComputeFogFactor(OUT.positionCS.z);

                                    half3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                                    half  sign = IN.tangentOS.w * GetOddNegativeScale();
                                    OUT.bitangentWS = cross(OUT.normalWS, tangentWS) * sign;
                                    OUT.tangentWS = tangentWS;

                                    return OUT;
                                }

                                half4 frag(Varyings IN) : SV_Target
                                {
                                    // ---------- ƒистанционный фейд ----------
                                    float dist = distance(IN.positionWS, _WorldSpaceCameraPos.xyz);
                                    float alpha; // вклад фейда

                                    if (dist <= _FadeEnd)
                                        alpha = 0.0;
                                    else if (dist >= _FadeStart)
                                        alpha = 1.0;
                                    else
                                    {
                                        float t = (dist - _FadeEnd) / (_FadeStart - _FadeEnd);
                                        alpha = pow(saturate(t), _FadeCurve);
                                    }

                                    // ---------- —борка SurfaceData (все карты Ч с общим UV) ----------
                                    half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                                    half3 albedo = baseMap.rgb * _BaseColor.rgb;

                                    half3 normalWS = normalize(IN.normalWS);

                                    #ifdef _NORMALMAP
                                        half3x3 tangentToWorld = half3x3(
                                            IN.tangentWS, IN.bitangentWS, IN.normalWS);

                                        half3 normalTS = UnpackNormalScale(
                                            SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv), _BumpScale);
                                        normalWS = normalize(TransformTangentToWorld(normalTS, tangentToWorld));
                                    #endif

                                    SurfaceData surfaceData = (SurfaceData)0;
                                    surfaceData.albedo = albedo;
                                    surfaceData.metallic = _Metallic;
                                    surfaceData.smoothness = _Smoothness;
                                    surfaceData.occlusion = 1.0;
                                    surfaceData.normalTS = half3(0, 0, 1);

                                    #ifdef _METALLICSPECGLOSSMAP
                                        half4 mg = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, IN.uv);
                                        surfaceData.metallic = mg.r;
                                        surfaceData.smoothness = mg.a * _Smoothness;
                                    #endif

                                    #ifdef _OCCLUSIONMAP
                                        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, IN.uv).g;
                                        surfaceData.occlusion = lerp(1.0h, occ, _OcclusionStrength);
                                    #endif

                                    #ifdef _EMISSION
                                        surfaceData.emission = half3(
                                            SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, IN.uv).rgb * _EmissionColor.rgb);
                                    #endif

                                        // ---------- ќсвещение (PBR) ----------
                                        InputData inputData = (InputData)0;
                                        inputData.positionWS = IN.positionWS;
                                        inputData.normalWS = normalWS;
                                        inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
                                        inputData.bakedGI = SampleSH(normalWS);
                                        inputData.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                                        inputData.shadowMask = half4(1, 1, 1, 1);
                                        inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

                                        half4 color = UniversalFragmentPBR(inputData, surfaceData);

                                        // ---------- “уман ----------
                                        color.rgb = MixFog(color.rgb, IN.fogFactor);

                                        // ---------- ‘инальна€ альфа ----------
                                        half mapAlpha = 1.0h;
                                        #ifdef _ALPHA_FROM_MAP
                                            mapAlpha = baseMap.a;
                                        #endif

                                        color.a = _BaseColor.a * mapAlpha * alpha;
                                        return color;
                                    }
                                    ENDHLSL
                                }
                        }
                            FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
