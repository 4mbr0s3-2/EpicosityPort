sampler uImage0 : register(s0);
float3 uColor;
float uSaturation;
float3 uSecondaryColor;
float uTime;
float uOpacity;
float4 uSourceRect;
float2 uWorldPosition;
float2 uImageSize0;
float2 uImageSize1;

float4 ComboMeterFullComboEffect(float2 coords : TEXCOORD0) : COLOR0
{
    float4 baseColor = tex2D(uImage0, coords);
    // Pixel should be brightened
    // uColor.r = x-coord of line to left
    if (coords.x + coords.y / 16 > uColor.r) 
    {
        float distanceFromLine = (coords.x - uColor.r) / 0.9;
        if (distanceFromLine > 1)
        {
            distanceFromLine = 1;
        }
        float intensity = 1 - distanceFromLine;
        baseColor.rgb *= 1 + intensity * 2 * uColor.r;
    }
    return baseColor;
}

technique Technique1
{
    pass ComboMeterFullComboEffect
    {
        PixelShader = compile ps_2_0 ComboMeterFullComboEffect();
    }
}