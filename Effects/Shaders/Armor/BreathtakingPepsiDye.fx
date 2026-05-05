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

float4 BreathtakingPepsiDye(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
	float luminosity = (color.r + color.g + color.b) / 3;

	//float pepsiWhiteStripeCenterY = sin(coords.x * 6.28f + uTime) * 0.2f + 0.5f;
	float pepsiRedFillY = sin(coords.x * 6.28f + uTime) * 0.1f + 0.5f + 0.1f * cos(uTime);
	float pepsiBlueFillY = cos(coords.x * 6.28f + uTime * 1.2f) * 0.1f + 0.5f - 0.2f * sin(uTime);
	
	float byteFraction = 0.00392156862f;

	float3 logoColor;

	logoColor.rgb = 0, 0, 0;

	if (frameY < pepsiRedFillY && frameY < pepsiBlueFillY)
	{
		logoColor.r = 235 * byteFraction;
		logoColor.g = 25 * byteFraction;
		logoColor.b = 51 * byteFraction;
	}
	else if (frameY > pepsiBlueFillY && frameY > pepsiRedFillY)
	{
		logoColor.r = 33 * byteFraction;
		logoColor.g = 81 * byteFraction;
		logoColor.b = 161 * byteFraction;
	}
	else
	{
		logoColor.rgb = 1, 1, 1;
	}

	color.rgb = luminosity * logoColor;

	return color * sampleColor;
}

technique Technique1
{
	pass BreathtakingPepsiDye
	{
		PixelShader = compile ps_2_0 BreathtakingPepsiDye();
	}
}