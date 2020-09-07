using Verse;

namespace Desynchronized.TNDBS.Datatypes
{
    public class LocationInfo : IExposable
    {
        private Map mapInWorld;
        private IntVec3 positionInMap;

        public Map Map
        {
            get
            {
                return mapInWorld;
            }
        }

        public IntVec3 Position
        {
            get
            {
                return positionInMap;
            }
        }

        public LocationInfo()
        {

        }

        public LocationInfo(Map map, IntVec3 position)
        {
            mapInWorld = map;
            positionInMap = position;
        }

        public static LocationInfo EmptyLocationInfo => new LocationInfo(null, IntVec3.Invalid);

        public void ExposeData()
        {
            Scribe_References.Look(ref mapInWorld, "mapInWorld");
            Scribe_Values.Look(ref positionInMap, "positionInMap", IntVec3.Invalid);
        }

        public bool IsForgotten()
        {
            if (Map == null)
            {
                return true;
            }
            if (Position == IntVec3.Invalid)
            {
                return true;
            }
            return false; 
        }
    }
}
