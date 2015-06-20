sampler s0;
bool active;
texture lightMask;
sampler lightSampler = sampler_state{ Texture = lightMask; };

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	float4 lightColor = tex2D(lightSampler, coords);
	if (active)
	{
		return color * (lightColor * 1.2f);
	}
	else
	{
		return color;
	}

}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
