//Prevent Unity warnings that are not relevant for this shader-------------------
#pragma warning (disable : 3571)  // pow(f, e) will not work for negative f, use abs(f) or conditionally handle negative values if you expect them
#pragma warning (disable : 4008)  // floating point division by zero
#pragma warning (disable : 3206)  // implicit truncation of vector type
//BLURS-------------------------------------------------------------------------
half4 Blur(half2 uv, sampler2D source, half Intensity)
{
	const half2 texelSize = 1.0 / _ScreenParams.xy;
	const half4 color = tex2D(source, uv);
	const half2 offset = Intensity * texelSize;

	half4 accumulatedColor = color;
	half accumulatedWeight = 1.0;
	for (int x = -1; x <= 1; x++)
	{
		for (int y = -1; y <= 1; y++)
		{
			const half2 sampleUV = uv + half2(x, y) * offset;
			const half4 sampleColor = tex2D(source, sampleUV);

			accumulatedColor += sampleColor;
			accumulatedWeight += 1.0;
		}
	}

	half4 blurredColor = accumulatedColor / accumulatedWeight;
	return blurredColor;
}

half BlurHD_G(half bhqp, half x)
{
	return exp(-(x * x) / (2.0 * bhqp * bhqp));
}
half4 BlurHD(half2 uv, sampler2D source, half BlurAmount, half xScale, half yScale)
{
	int iterations = 16;
	int halfIterations = iterations / 2;
	half sigmaX = 0.1 + BlurAmount * 0.5;
	half sigmaY = sigmaX;
	half total = 0.0;
	half4 ret = half4(0, 0, 0, 0);
	for (int iy = 0; iy < iterations; ++iy)
	{
		half fy = BlurHD_G(sigmaY, half(iy) -half(halfIterations));
		half offsetY = half(iy - halfIterations) * 0.00390625 * xScale;
		for (int ix = 0; ix < iterations; ++ix)
		{
			half fx = BlurHD_G(sigmaX, half(ix) - half(halfIterations));
			half offsetX = half(ix - halfIterations) * 0.00390625 * yScale;
			total += fx * fy;
			ret += tex2D(source, uv + half2(offsetX, offsetY)) * fx * fy;
		}
	}
	return ret / total;
}
//-----------------------------------------------------------------------
half rand(half2 seed, half offset) {
	return (frac(sin(dot(seed, half2(12.9898, 78.233))) * 43758.5453) + offset) % 1.0;
}

half rand2(half2 seed, half offset) {
	return (frac(sin(dot(seed * floor(50 + (_Time.x % 1.0) * 12.), half2(127.1, 311.7))) * 43758.5453123) + offset) % 1.0;
}

half rand2CustomTime(half2 seed, half offset, half customTime) {
	return (frac(sin(dot(seed * floor(50 + (customTime % 1.0) * 12.), half2(127.1, 311.7))) * 43758.5453123) + offset) % 1.0;
}
//-----------------------------------------------------------------------
half RemapFloat(half inValue, half inMin, half inMax, half outMin, half outMax){
	return outMin + (inValue - inMin) * (outMax - outMin) / (inMax - inMin);
}