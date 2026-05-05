sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 PewDieDye(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 imageCoords = coords;
    
    // Get y-coordinate based on sourceRect
    float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;

    // Get x pixel of drawData
    float imageCoordsPixelX = uImageSize0.x * imageCoords.x;
    // Divide by width of PewDiePie pattern in order to keep size of pattern consistent, set imageCoords.x
    imageCoords.x = imageCoordsPixelX / uImageSize1.x;
    // Instead of mapping Y coordinates between drawData and pattern, squash Y coordinates and make pattern height match drawData
    imageCoords.y = frameY;

    float sineOffset = uTime % (3.141593f * 2);
    float sineAmplitude = cos(sineOffset) * 0.2f + 1;

    // Waves
    imageCoords.y += sin(imageCoords.x * 0.6f + sineOffset) * 0.7f * sineAmplitude;
    // Tighten waves
    imageCoords.x *= 1.3f * sineAmplitude;
    
    // Wrap image
    imageCoords.x %= 1;
    imageCoords.y %= 1;
    
    float4 pewDiePieColor = tex2D(uImage1, imageCoords);
    
    float4 originalColor = tex2D(uImage0, coords);
    float luminosity = (originalColor.r + originalColor.g + originalColor.b) / 3;
    originalColor.rgb = luminosity * uColor;

    return originalColor * pewDiePieColor * sampleColor;
}

technique Technique1
{
    pass PewDieDye
    {
        PixelShader = compile ps_2_0 PewDieDye();
    }
}