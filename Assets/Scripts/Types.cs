using System;

public class Types
{
    [Flags]
    public enum Terrain
    {
        nothing =0,
        grass = 1 << 0,
        sand = 1 << 1,
        swamp = 1 << 2,
        water = 1 << 3,
    }
}
