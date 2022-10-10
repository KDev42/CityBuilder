public static class Transformation
{
    public static bool CanTurnInto(Types.Terrain oldTerrain, out Types.Terrain newTerrain)
    {
        switch(oldTerrain)
        {
            case Types.Terrain.water:
                newTerrain = Types.Terrain.swamp;
                return true;
            case Types.Terrain.swamp:
                newTerrain = Types.Terrain.sand;
                return true;
        }

        newTerrain = Types.Terrain.nothing;
        return false;
    }
}
