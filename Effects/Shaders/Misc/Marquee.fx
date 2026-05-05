sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float uSaturation;
float3 uSecondaryColor;
float uTime;
float uOpacity;
float4 uSourceRect;
float2 uWorldPosition;
float2 uImageSize0;
float2 uImageSize1;

float4 Marquee(float2 coords : TEXCOORD0) : COLOR0
{
    float speed = uColor.r;
    float2 sampleCoords = coords;
    sampleCoords.x += uTime * speed;
    sampleCoords.x = frac(sampleCoords.x);
    float4 mapColor = tex2D(uImage0, sampleCoords);
    
    return mapColor;
}

technique Technique1
{
    pass Marquee
    {
        PixelShader = compile ps_2_0 Marquee();
    }
}