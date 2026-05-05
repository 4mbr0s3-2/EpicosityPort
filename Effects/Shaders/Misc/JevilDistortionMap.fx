sampler uImage0 : register(s0);
texture mapping;
sampler mappingSampler = sampler_state { Texture = mapping; };
float3 uColor;
float uSaturation;
float3 uSecondaryColor;
float uTime;
float uOpacity;
float4 uSourceRect;
float2 uWorldPosition;
float2 uImageSize0;
float2 uImageSize1;

float4 JevilDistortionMap(float2 coords : TEXCOORD0) : COLOR0
{
    // There's definitely a better way of doing this without a texture
    float speed = uColor.r;
    //float2 movedCoords = coords;
    float byteFraction = 1.0f / 255.0f;
    float4 mapColor = tex2D(mappingSampler, coords);
    if (mapColor.b > 0) 
    {
        return float4(0, 0, 0, 0);
    }
    float2 sampledCoords = mapColor.rg;
    sampledCoords.x += uTime * speed;
    sampledCoords.x = frac(sampledCoords.x);
    float4 color = tex2D(uImage0, sampledCoords);
    float4 baseColor = float4(20 * byteFraction, 22 * byteFraction, 66 * byteFraction, 255 * byteFraction);
    baseColor.rgb *= color.r;
    baseColor.rgba *= uColor.g;
    return baseColor;
}

technique Technique1
{
    pass JevilDistortionMap
    {
        PixelShader = compile ps_2_0 JevilDistortionMap();
    }
}