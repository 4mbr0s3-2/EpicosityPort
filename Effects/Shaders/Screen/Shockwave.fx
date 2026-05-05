sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

// Shockwave effect by Kazzymodus https://forums.terraria.org/index.php?threads/tutorial-shockwave-effect-for-tmodloader.81685/
float4 Shockwave(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float PI = 3.1415926535f;
    // Relative coords to screen
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    // Vector from current coords to target coords (in 1:1 ratio)
    float2 centreCoords = (coords - targetCoords) * (uScreenResolution / uScreenResolution.y);
    // Square of distance (a^2 + b^2)
    float dotField = dot(centreCoords, centreCoords);
    // uColor.x = number of ripples
    // uColor.y = ripple size (stretches squared distance from target to coords)
    // uColor.z = wave speed
    float ripple = dotField * uColor.y * PI - uProgress * uColor.z;

    if (ripple < 0 && ripple > uColor.x * -2 * PI)
    {
        ripple = saturate(sin(ripple));
    }
    else
    {
        ripple = 0;
    }

    float2 sampleCoords = coords + ((ripple * uOpacity / uScreenResolution) * centreCoords);
    return tex2D(uImage0, sampleCoords);
}

technique Technique1
{
    pass Shockwave
    {
        PixelShader = compile ps_2_0 Shockwave();
    }
}