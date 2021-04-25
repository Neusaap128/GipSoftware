
using UnityEngine;
using System.Linq;

namespace RiskOfRain{

    [System.Serializable]
    public class Region
    {

        public static Region largestRegion;

        public Tile[] tiles;

        public bool accessible;

        public int minX, maxX;
        public int minY, maxY;
        public int regionSize;
    
        public Region(Tile[] tiles, int minX, int maxX, int minY, int maxY)
        {
            this.tiles = tiles;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            regionSize = tiles.Length;
        }

        public bool DoesTileLieInRegion(Tile tile){
            return tiles.ToList().Contains(tile);
        }
        

    }

}