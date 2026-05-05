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
texture spiral;
sampler spiralSampler = sampler_state { Texture = spiral; };

float4 HypnosisDye(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float luminosity = (color.r + color.g + color.b) / 3;

	float scaleFactor = uImageSize1.y / uSourceRect.w;
	if (uSourceRect.z > uSourceRect.w)
	{
		scaleFactor = uImageSize1.y / uSourceRect.z;
	}
	
	float pixelX = (coords.x - 0.5) * uImageSize0.x * scaleFactor;
	float pixelY = (coords.y * uImageSize0.y - uSourceRect.y) * scaleFactor;

	float2 sampleCoords = float2((pixelX + uImageSize1.x * 0.5) / uImageSize1.x, pixelY / uImageSize1.y);
	
	float x = sampleCoords.x - 0.5;
	float y = sampleCoords.y - 0.5;

	float angle = -uTime * 8;

	sampleCoords = float2(x * cos(angle) - y * sin(angle) + 0.5, x * sin(angle) + y * cos(angle) + 0.5);
	if (sampleCoords.x > 1 || sampleCoords.x < 0) 
	{
		sampleCoords.x += 0.5;
		sampleCoords.x /= 2;
	}
	if (sampleCoords.y > 1 || sampleCoords.y < 0)
	{
		sampleCoords.y += 0.5;
		sampleCoords.y /= 2;
	}
	float4 spiralColor = tex2D(spiralSampler, sampleCoords);
	float4 lumColor = float4(luminosity, luminosity, luminosity, 1);

	return spiralColor * sampleColor.a * lumColor * color.a;
}

technique Technique1
{
	pass HypnosisDye
	{
		PixelShader = compile ps_2_0 HypnosisDye();
	}
}