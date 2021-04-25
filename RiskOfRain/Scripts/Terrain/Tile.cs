
using UnityEngine.Tilemaps;
using UnityEngine;

namespace RiskOfRain {

    public class Tile{

        public int x, y;
        public bool wall { get; private set; }
        public WallSides edges;

        public TerrainSprite sprite;

        public Region region;

        public Tile(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void SetWall(bool value){
            wall = value;
            sprite = value ? TerrainGenerator.Sprites.wallTileBase : null;
        }

    }

    [System.Flags]
    public enum WallSides : byte
    {
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8
    }

}