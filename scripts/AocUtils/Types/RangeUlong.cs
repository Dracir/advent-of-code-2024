namespace AocUtils;

public struct RangeUlong
{
	public ulong Min;
	public ulong MaxExclusive;

	public RangeUlong(ulong min, ulong maxExclusive)
	{
		Min = min;
		MaxExclusive = maxExclusive;
	}

	public readonly ulong Width => MaxExclusive - Min;

	public readonly bool Contains(ulong value) => value >= Min && value <= MaxExclusive;

}