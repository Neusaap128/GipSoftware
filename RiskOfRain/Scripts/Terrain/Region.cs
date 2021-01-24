
using UnityEngine;

namespace RiskOfRain{

    public class Region
    {
    
        public Tile[] tiles;

        public bool isSurface = false;

        public int minX, maxX;
        public int minY, maxY;
    
        public Region(Tile[] tiles, bool isSurface, int minX, int maxX, int minY, int maxY)
        {
            this.tiles = tiles;
            this.isSurface = isSurface;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;

        }
    
    }

}