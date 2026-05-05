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

float4 WorkInProgressRarity(float2 coords : TEXCOORD0) : COLOR0
{
	// Normalized pixel coordinates (from 0 to 1)
	float numRows = 20.0;
	float2 uv = coords;
	uv.x += uv.y * 0.3 + uTime * 0.1;
	float3 yellow = float3(1.0, 1.0, 0);
	// Time varying pixel color
	float2 rows = frac(uv * numRows);
	float3 col = step(rows.x, 0.5) * yellow * tex2D(uImage0, coords).r;
	// Output to screen
	return float4(col, tex2D(uImage0, coords).a);
}

technique Technique1
{
	pass WorkInProgressRarity
	{
		PixelShader = compile ps_2_0 WorkInProgressRarity();
	}
}