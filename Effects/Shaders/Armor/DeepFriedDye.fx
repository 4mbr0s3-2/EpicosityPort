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

float4 DeepFriedDye(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1 * 2;
	
	noiseCoords.x += uTime * 6;
	noiseCoords.y += uTime * 7;
	noiseCoords.x %= 1;
	noiseCoords.y %= 1;

	float4 noise = tex2D(uImage1, noiseCoords);
	
	float noisePercent = 0.6f;

	float4 color = tex2D(uImage0, coords);


	
	if (color.r > 0.5) 
	{
		color.r = 0.9;
	}
	else 
	{
		color.r = 0;
	}
	if (color.g > 0.5)
	{
		color.g = 0.9;
	}
	else
	{
		color.g = 0;
	}
	if (color.b > 0.5)
	{
		color.b = 0.9;
	}
	else
	{
		color.b = 0;
	}

	color.r += noise.r * noisePercent;
	color.g += noise.g * noisePercent;
	color.b += noise.b * noisePercent;

	return color * sampleColor * color.a;
}

technique Technique1
{
	pass DeepFriedDye
	{
		PixelShader = compile ps_2_0 DeepFriedDye();
	}
}